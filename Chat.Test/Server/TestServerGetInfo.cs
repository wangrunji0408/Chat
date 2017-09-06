using System.Collections.Generic;
using System.Linq;
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
        
        [Fact]
        public async Task GetChatroomInfo()
        {
            var info = await server.GetChatroomInfoAsync(userId: 1, roomId: 1);
            Assert.Equal(1, info.Id);
            Assert.Equal("Global Chatroom", info.Name);
            Assert.Equal(new HashSet<long> {1, 2}, info.PeopleIds.ToHashSet());
        }
        
        [Fact]
        public async Task GetChatroomInfo_NotInside()
        {
            var room = await server.NewChatroomAsync(new long[]{2});
            var info = await server.GetChatroomInfoAsync(userId: 1, roomId: room.Id);
            Assert.Equal(room.Id, info.Id);
            Assert.Equal("", info.Name);
            Assert.Equal(0, info.PeopleIds.Count);
        }
    }
}