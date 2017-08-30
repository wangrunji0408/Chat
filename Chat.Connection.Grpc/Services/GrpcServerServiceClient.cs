using System;
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
                throw new Exception($"Failed to login. [{response.Status}] {response.Detail}");
            logged = true;
            userId = request.UserId;

            await RegisterClient();

            return this;
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

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return await base.SignupAsync(request);
        }

        public IAsyncEnumerable<ChatMessage> GetMessageAfter(DateTimeOffset time)
        {
			var request = new GetMessagesRequest
			{
				UserId = userId,
				AfterTimeUnix = time.ToUnixTimeSeconds()
			};
            var reader = base.GetMessages(request).ResponseStream;
            return AsyncEnumerable.CreateEnumerable(() => reader);
        }
    }
}