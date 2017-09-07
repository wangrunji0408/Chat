using System;
using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Server
{
    public class TestFriend : TestServerBase
    {
        [Fact]
        public async Task MakeFriends()
        {
            var user1 = await server.FindUserAsync(1);
            var user2 = await server.FindUserAsync(2);
            Assert.DoesNotContain(2, user1.FriendIds);
            Assert.DoesNotContain(1, user2.FriendIds);
            await server.MakeFriends(1, 2);
            Assert.Contains(2, user1.FriendIds);
            Assert.Contains(1, user2.FriendIds);
        }

        [Fact]
        public async Task AlreadyFriend()
        {
            await server.MakeFriends(1, 2);
            await Assert.ThrowsAsync<InvalidOperationException>(() => server.MakeFriends(1, 2));
        }
    }
}