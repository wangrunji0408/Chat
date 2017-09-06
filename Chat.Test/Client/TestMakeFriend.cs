using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public abstract class TestMakeFriend<TSetup>: TestClientBase<TSetup>
        where TSetup: SetupBase, new()
    {
        private Client.Client client1;
        private Client.Client client2;
	    
        public TestMakeFriend()
        {
            loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" }).Wait();
            loginService.SignupAsync(new SignupRequest { Username = "user2", Password = "123456" }).Wait();
            client1 = clientBuilder.SetUser(1, "123456").Build();
            client2 = clientBuilder.SetUser(2, "123456").Build();
            client1.Login().Wait();
            client2.Login().Wait();
        }
        
        [Fact]
        public async Task TargetReceive()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            var received = false;
            client2.MakeFriendHandler = async request =>
            {
                received = true;
                Assert.Equal("Hello", request.Greeting);
                Assert.Equal(1, request.SenderId);
                Assert.Equal(2, request.TargetId);

                return accept;
            };
            await client1.MakeFriend(2, "Hello");
            Assert.True(received);
        }
        
        [Fact]
        public async Task Accept()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = async request => accept;
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(accept, response);
        }
        
        [Fact]
        public async Task Refuse()
        {
            var refuse = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Refuse};
            client2.MakeFriendHandler = async request => refuse;
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(refuse, response);
        }
        
        [Fact]
        public async Task UserNotExist()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = async request => accept;
            var response = await client1.MakeFriend(3, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.UserNotExist
            }, response);
        }
        
        [Fact]
        public async Task AlreadyFriend()
        {
            await server.MakeFriends(1, 2);
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = async request => accept;
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.AlreadyFriend
            }, response);
        }
        
        [Fact]
        public async Task WithSelf()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = async request => accept;
            var response = await client1.MakeFriend(1, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.WithSelf
            }, response);
        }
    }
    
    public class TestMakeFriend_Local: TestMakeFriend<LocalSetup> {}
    public class TestMakeFriend_GrpcLocal: TestMakeFriend<GrpcLocalSetup> {}
}