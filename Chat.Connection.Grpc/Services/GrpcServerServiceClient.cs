using System;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Grpc.Core;

namespace Chat.Connection.Grpc
{
    public class GrpcServerServiceClient: ChatServerService.ChatServerServiceClient, IServerService, ILoginService
    {
        private bool logged;
        private long userId;
        
        public GrpcServerServiceClient(string target):
            base(new Channel(target, ChannelCredentials.Insecure))
        {
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            var request = new SendMessageRequest
            {
                Message = message
            };
            await base.SendMessageAsync(request);
        }

        public async Task<IServerService> LoginAsync(LoginRequest request)
        {
            if(logged)
                throw new InvalidOperationException("Can only be login once.");
            var response = await base.LoginAsync(request);
            if(response.Status != LoginResponse.Types.Status.Success)
                throw new Exception($"Failed to login. {response.Detail}");
            logged = true;
            userId = request.UserId;
            return this;
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return await base.SignupAsync(request);
        }
    }
}