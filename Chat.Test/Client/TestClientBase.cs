using Chat.Client;
using Chat.Core.Interfaces;
using Chat.Core.Models;

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
            SignupAndLogin();
        }

        private void Setup()
        {
            var setup = new TSetup();
            loginService = setup.loginService;
            clientBuilder = setup.clientBuilder;
            server = setup.server;
        }

        protected virtual void SignupAndLogin()
        {
            loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "123456"}).Wait();
            loginService.SignupAsync(new SignupRequest {Username = "user2", Password = "123456"}).Wait();
            client1 = clientBuilder.SetUser(1, "123456").Build();
            client2 = clientBuilder.SetUser(2, "123456").Build();
            client1.Login().Wait();
            client2.Login().Wait();
        }
    }
}