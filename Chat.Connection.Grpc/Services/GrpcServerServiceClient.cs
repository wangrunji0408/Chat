﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Grpc.Core;

namespace Chat.Connection.Grpc
{
    class GrpcServerServiceClient: ChatServerService.ChatServerServiceClient, IServerService, ILoginService
    {
        private bool logged;
        private long userId;
        internal GrpcClientServiceImpl ClientService { get; set; }
        
        public GrpcServerServiceClient(string target):
            base(new Channel(target, ChannelCredentials.Insecure))
        {
        }

        public async Task<SendMessageResponse> SendMessageAsync(ChatMessage message)
        {
            var request = new SendMessageRequest
            {
                Message = message
            };
            return await base.SendMessageAsync(request);
        }


        async Task RegisterClient ()
        {
            var request = new RegisterAddressRequest 
            {
                UserId = userId, 
                Address = $"{ClientService.Host}:{ClientService.Port}"
            };
			var response = await base.RegisterAddressAsync(request);
			if (response.Success == false)
				throw new Exception($"Failed to register client service. {response.Detail}");
        }

        public async Task<IServerService> GetService(LoginResponse token)
        {
            if(!token.Success)
                throw new Exception($"Failed to login. {token.Detail}");
            logged = true;
            userId = token.UserId;

            await RegisterClient();

            return this;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if(logged)
                throw new InvalidOperationException("Can only be login once.");
            return await base.LoginAsync(request);
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return await base.SignupAsync(request);
        }

        public IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request)
        {
            var reader = base.GetMessages(request).ResponseStream;
            return AsyncEnumerable.CreateEnumerable(() => reader);
        }

        public async Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request)
        {
            return await base.GetChatroomInfoAsync(request);
        }

        public async Task<GetPeopleInfoResponse> GetPeopleInfo(GetPeopleInfoRequest request)
        {
            return await base.GetPeoplesInfoAsync(request);
        }

        public async Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
        {
            return await base.MakeFriendAsync(request);
        }

        public IAsyncEnumerable<GetDataResponse> GetData(GetDataRequest request)
        {
            var reader = base.GetData(request).ResponseStream;
            return AsyncEnumerable.CreateEnumerable(() => reader);
        }
    }
}