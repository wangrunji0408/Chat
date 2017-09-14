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
        private Metadata _metadata = new Metadata();
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
            return await base.SendMessageAsync(request, _metadata);
        }

        // For test
        internal void SetToken(string token)
        {
            _metadata = new Metadata
            {
                {"id", userId.ToString()},
                {"token", token}
            };
        }

        async Task RegisterClient ()
        {
            var request = new RegisterAddressRequest 
            {
                UserId = userId, 
                Address = $"{ClientService.Host}:{ClientService.Port}"
            };
			var response = await base.RegisterAddressAsync(request, _metadata);
			if (response.Success == false)
				throw new Exception($"Failed to register client service. {response.Detail}");
        }

        public async Task<IServerService> GetService(LoginResponse token)
        {
            if(!token.Success)
                throw new Exception($"Failed to login. {token.Detail}");
            userId = token.UserId;
            _metadata.Add("token", token.Token);
            _metadata.Add("id", token.UserId.ToString());
            await RegisterClient();

            return this;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if(logged)
                throw new InvalidOperationException("Can only be login once.");
            logged = true;
            return await base.LoginAsync(request, _metadata);
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return await base.SignupAsync(request, _metadata);
        }

        public IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request)
        {
            var reader = base.GetMessages(request, _metadata).ResponseStream;
            return AsyncEnumerable.CreateEnumerable(() => reader);
        }

        public async Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request)
        {
            return await base.GetChatroomInfoAsync(request, _metadata);
        }

        public async Task<GetPeopleInfoResponse> GetPeopleInfo(GetPeopleInfoRequest request)
        {
            return await base.GetPeoplesInfoAsync(request, _metadata);
        }

        public async Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
        {
            return await base.MakeFriendAsync(request, _metadata);
        }

        public IAsyncEnumerable<GetDataResponse> GetData(GetDataRequest request)
        {
            var reader = base.GetData(request, _metadata).ResponseStream;
            return AsyncEnumerable.CreateEnumerable(() => reader);
        }
    }
}