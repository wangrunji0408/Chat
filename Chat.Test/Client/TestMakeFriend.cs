using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Test.Client;
using Xunit;

namespace Chat.Test
{
    public abstract class TestMakeFriend<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        private void AssertFriend(bool isfriend, long id1, long id2)
        {
            var user1 = server.FindUserAsync(id1).Result;
            var user2 = server.FindUserAsync(id2).Result;
            Assert.Equal(isfriend, user1.IsFriend(user2));
            Assert.Equal(isfriend, user2.IsFriend(user1));
        }

        [Fact]
        public async Task Accept()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = request => Task.FromResult(accept);
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(accept, response);
            AssertFriend(true, 1, 2);
        }

        [Fact]
        public async Task AlreadyFriend()
        {
            await server.MakeFriendsAsync(1, 2);
            AssertFriend(true, 1, 2);
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = request => Task.FromResult(accept);
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.AlreadyFriend
            }, response);
        }

        [Fact]
        public async Task Refuse()
        {
            var refuse = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Refuse};
            client2.MakeFriendHandler = request => Task.FromResult(refuse);
            var response = await client1.MakeFriend(2, "Hello");
            Assert.Equal(refuse, response);
            AssertFriend(false, 1, 2);
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
        public async Task UserNotExist()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = request => Task.FromResult(accept);
            var response = await client1.MakeFriend(3, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.UserNotExist
            }, response);
        }

        [Fact]
        public async Task WithSelf()
        {
            var accept = new MakeFriendResponse {Status = MakeFriendResponse.Types.Status.Accept};
            client2.MakeFriendHandler = request => Task.FromResult(accept);
            var response = await client1.MakeFriend(1, "Hello");
            Assert.Equal(new MakeFriendResponse
            {
                Status = MakeFriendResponse.Types.Status.WithSelf
            }, response);
        }
    }
}