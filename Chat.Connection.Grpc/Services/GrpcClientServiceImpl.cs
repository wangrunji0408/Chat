using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Connection.Grpc
{
    using Client;
    using static SendMessageResponse.Types.Status;
    
    public class GrpcClientServiceImpl: ChatClientService.ChatClientServiceBase
    {
        private readonly Client _client;
        private readonly ILogger _logger;
        readonly int _port;

        public GrpcClientServiceImpl(
            Client client,
            GrpcServerServiceClient ssc,
            IServiceProvider serviceProvider,
            int port = 8080)
        {
            _client = client;
			_port = port;
			_logger = serviceProvider.GetService<ILoggerFactory>()?
                                     .CreateLogger($"GrpcServerService for User {client.UserId}");

            StartGrpcServer(port);
            RegisterSelf(ssc);
        }

        private void StartGrpcServer(int port)
        {
            var grpcServer = new global::Grpc.Core.Server
            {
                Services = { ChatClientService.BindService(this) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            grpcServer.Start();
            _logger?.LogInformation($"Start gRPC Server @ localhost:{port}");
        }

        private void RegisterSelf (GrpcServerServiceClient ssc)
        {
            var request = new RegisterAddressRequest { UserId = _client.UserId, Address = _port.ToString() };
            var response = ssc.RegisterAddress(request);
            if (response.Success == false)
                throw new Exception($"Failed to register client service. {response.Detail}");
        }

        public override async Task<SendMessageResponse> NewMessage(SendMessageRequest request, ServerCallContext context)
        {
            _client.InformNewMessage(request.Message);
            return new SendMessageResponse{Status = Success};
        }
    }
}