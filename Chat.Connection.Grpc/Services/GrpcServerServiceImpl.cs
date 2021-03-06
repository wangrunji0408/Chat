﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chat.Core.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Connection.Grpc
{
    using Server;
    using Core.Interfaces;

    class GrpcServerServiceImpl : ChatServerService.ChatServerServiceBase
    {
        private readonly Server _server;
        private readonly ILogger _logger;

        public GrpcServerServiceImpl(
            Server server,
            IServiceProvider serviceProvider,
            string host, int port)
        {
            _server = server;
            _logger = serviceProvider.GetService<ILoggerFactory>()?
                .CreateLogger("Chat.GrpcServerService");
            
			if (port == 0)
				port = Util.FreeTcpPort();
            var grpcServer = new global::Grpc.Core.Server
            {
                Services = {ChatServerService.BindService(this)},
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };
            grpcServer.Start();
            _logger?.LogInformation($"Start gRPC Server @ {host}:{port}");
        }

        string GetValue(IEnumerable<Metadata.Entry> list, string key)
        {
            return list
                .Where(e => e.Key == key)
                .Select(e => e.Value)
                .FirstOrDefault();
        }
        
        async Task<long> VertifyTokenAsync(ServerCallContext context)
        {
            var token = GetValue(context.RequestHeaders, "token");
            var id = long.Parse(GetValue(context.RequestHeaders, "id"));
            var ok = await _server.VertifyTokenAsync(id, token);
            if(!ok)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Failed to vertify token."));
            return id;
        }
        
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            _logger?.LogInformation($"New login request from {context.Peer}");
            return _server.LoginAsync(request);
        }

        public override async Task<Response> RegisterAddress(RegisterAddressRequest request, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
			var target = request.Address;
            //var target = context.Peer;
            //target = target.Remove(target.LastIndexOf(':') + 1) + port;

            var client = new GrpcClientServiceClient(target);
            _server.SetUserClient(request.UserId, client);
            _logger?.LogInformation($"User {request.UserId} register address: {target}");
            return new Response { Success = true };
        }

        public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request,
            ServerCallContext context)
        {
            var id = await VertifyTokenAsync(context);
            request.Message.SenderId = id;
            return await _server.ReceiveNewMessageAsync(request.Message);
        }

        public override Task<SignupResponse> Signup(SignupRequest request, ServerCallContext context)
        {
			return _server.SignupAsync(request);
        }

        public override async Task GetMessages(GetMessagesRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
            var messages = await _server.GetMessagesAsync(request);
            foreach (var message in messages)
                await responseStream.WriteAsync(message);
        }

        public override async Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
            var info = await _server.GetChatroomInfoAsync(request.SenderId, request.ChatroomId);
            return new GetChatroomInfoResponse{Success = true, Chatroom = info};
        }

        public override async Task<GetPeopleInfoResponse> GetPeoplesInfo(GetPeopleInfoRequest request, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
            var info = await _server.GetPeopleInfoAsync(request.SenderId, request.TargetId);
            return new GetPeopleInfoResponse{PeopleInfo = info};
        }

        public override async Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
            return await _server.MakeFriendsAsync(request);
        }

        public override async Task GetData(GetDataRequest request, IServerStreamWriter<GetDataResponse> responseStream, ServerCallContext context)
        {
            await VertifyTokenAsync(context);
            var responses = _server.GetDataAsync(request);
            var enumor = responses.GetEnumerator();
            while (await enumor.MoveNext(context.CancellationToken))
                await responseStream.WriteAsync(enumor.Current);
        }
    }
}