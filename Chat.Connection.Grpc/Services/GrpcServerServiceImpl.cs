using System;
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

    public class GrpcServerServiceImpl : ChatServerService.ChatServerServiceBase
    {
        private readonly Server _server;
        private readonly ILogger _logger;

        public GrpcServerServiceImpl(
            Server server,
            IServiceProvider serviceProvider,
            int port = 8080)
        {
            _server = server;
            _logger = serviceProvider.GetService<ILoggerFactory>()?
                .CreateLogger<GrpcServerServiceImpl>();

            var grpcServer = new global::Grpc.Core.Server
            {
                Services = {ChatServerService.BindService(this)},
                Ports = {new ServerPort("localhost", port, ServerCredentials.Insecure)}
            };
            grpcServer.Start();
            _logger?.LogInformation($"Start gRPC Server @ localhost:{port}");
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            _logger?.LogInformation($"New login request from {context.Peer}");
            return _server.Login(request);
        }

        public override async Task<Response> RegisterAddress(RegisterAddressRequest request, ServerCallContext context)
        {
            var port = request.Address;
            var target = context.Peer;
            target = target.Remove(target.LastIndexOf(':') + 1) + port;

            var client = new GrpcClientServiceClient(target);
            _server.SetUserClient(request.UserId, client);
            _logger.LogInformation($"User {request.UserId} register address: {target}");
            return new Response { Success = true };
        }

        public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request,
            ServerCallContext context)
        {
            _server.SendMessageAsync(request.Message);

            return new SendMessageResponse
            {
                Status = SendMessageResponse.Types.Status.Success
            };
        }

        public override async Task<SignupResponse> Signup(SignupRequest request, ServerCallContext context)
        {
            return _server.Signup(request);
        }
    }
}