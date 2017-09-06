using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public class TestServerFriend: TestServerBase
    {
        public TestServerFriend()
        {
            server.SignupAsync(new SignupRequest
            {
                Username = "user1",
                Password = "password"
            }).Wait();

            server.SignupAsync(new SignupRequest
            {
                Username = "user2",
                Password = "password"
            }).Wait();
        }

        [Fact]
        public async Task MakeFriends()
        {
            await server.MakeFriends(1, 2);
            var user1 = await server.FindUserAsync(1);
            var user2 = await server.FindUserAsync(2);
            Assert.Contains(2, user1.FriendIds);
            Assert.Contains(1, user2.FriendIds);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() => server.MakeFriends(1, 2));
        }
    }
}