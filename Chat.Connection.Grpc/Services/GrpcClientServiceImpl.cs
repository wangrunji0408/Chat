using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Chat.Core.Models;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Connection.Grpc
{
    
    using Client;
    using static SendMessageResponse.Types.Status;
    
    class GrpcClientServiceImpl: ChatClientService.ChatClientServiceBase
    {
        private readonly Client _client;
        private readonly ILogger _logger;
        internal int Port { get; private set; }
                           
        public GrpcClientServiceImpl(
            Client client,
            IServiceProvider serviceProvider,
            int port)
        {
            _client = client;
			Port = port;
			_logger = serviceProvider.GetService<ILoggerFactory>()?
                                     .CreateLogger($"GrpcServerService for User {client.UserId}");

            StartGrpcServer();
        }

        private void StartGrpcServer()
        {
            if (Port == 0)
                Port = FreeTcpPort();
			var grpcServer = new global::Grpc.Core.Server
			{
				Services = { ChatClientService.BindService(this) },
				Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
			};
            grpcServer.Start();
            _logger?.LogInformation($"Start gRPC Server @ localhost:{Port}");
        }

		// https://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
		static int FreeTcpPort()
		{
			var l = new TcpListener(IPAddress.Loopback, 0);
			l.Start();
			int port = ((IPEndPoint)l.LocalEndpoint).Port;
			l.Stop();
			return port;
		}

        public override async Task<SendMessageResponse> NewMessage(SendMessageRequest request, ServerCallContext context)
        {
			await Task.CompletedTask;

			_client.InformNewMessage(request.Message);
            return new SendMessageResponse{Status = Success};
        }
    }
}