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
            clientBuilder = setup.clientBuilder;
            server = setup.server;
        }

        protected virtual async Task SignupAndLoginAsync()
        {
            await clientBuilder.SignupAsync("user1", "password123");
            await clientBuilder.SignupAsync("user2", "password123");
            client1 = await clientBuilder.LoginAsync("user1", "password123");
            client2 = await clientBuilder.LoginAsync("user2", "password123");
        }
    }
}