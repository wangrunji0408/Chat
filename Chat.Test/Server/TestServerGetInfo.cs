using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public class TestServerGetInfo: TestServerBase
    {
        public TestServerGetInfo()
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
        public async Task GetPeopleInfo()
        {
            var info = await server.GetPeopleInfoAsync(userId: 1, targetId: 2);
            Assert.Equal(2, info.Id);
            Assert.Equal("user2", info.Username);
        }
    }
}