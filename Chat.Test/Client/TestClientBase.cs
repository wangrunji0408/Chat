using System.Threading.Tasks;
using Chat.Client;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        protected const long GlobalChatroomId = 1;

        protected Chat.Client.Client client1;
        protected Chat.Client.Client client2;
        protected ClientBuilder clientBuilder;
        protected ILoginService loginService;
        protected Chat.Server.Server server;

        public TestClientBase()
        {
            Setup();
            // ReSharper disable once VirtualMemberCallInConstructor
            SignupAndLoginAsync().Wait();
        }

        private void Setup()
        {
            var setup = new TSetup();
            loginService = setup.loginService;
            clientBuilder = setup.clientBuilder;
            server = setup.server;
        }

        protected virtual async Task SignupAndLoginAsync()
        {
            var rsp1 = await loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "password123"});
            var rsp2 = await loginService.SignupAsync(new SignupRequest {Username = "user2", Password = "password123"});
            Assert.True(rsp1.Success, rsp1.Detail);
            Assert.True(rsp2.Success, rsp2.Detail);
            client1 = await clientBuilder.Login("user1", "password123");
            client2 = await clientBuilder.Login("user2", "password123");
        }
    }
}