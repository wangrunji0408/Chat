using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.Models;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Connection.Grpc
{
    class GrpcClientServiceImpl: ChatClientService.ChatClientServiceBase
    {
        private readonly Client.Client _client;
        private readonly ILogger _logger;
        internal string Host { get; private set; }
        internal int Port { get; private set; }
                           
        public GrpcClientServiceImpl(
            Client.Client client,
            IServiceProvider serviceProvider,
            string host, int port)
        {
            _client = client;
			Port = port;
            Host = host;
			_logger = serviceProvider.GetService<ILoggerFactory>()?
                                     .CreateLogger($"Chat.GrpcServerService(UserId:{client.UserId})");

            StartGrpcServer();
        }

        private void StartGrpcServer()
        {
            if (Port == 0)
                Port = Util.FreeTcpPort();
			var grpcServer = new global::Grpc.Core.Server
			{
				Services = { ChatClientService.BindService(this) },
				Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
			};
            grpcServer.Start();
            _logger?.LogInformation($"Start gRPC Server @ {Host}:{Port}");
        }

        public override async Task<SendMessageResponse> NewMessage(SendMessageRequest request, ServerCallContext context)
        {
            try
            {
                _client.InformNewMessage(request.Message);
            }
            catch (Exception e)
            {
                return new SendMessageResponse{Success = false, Detail = e.Message};
            }
            return new SendMessageResponse{Success = true};
        }

        public override Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request, ServerCallContext context)
        {
            return _client.MakeFriendHandler(request);
        }
    }
}