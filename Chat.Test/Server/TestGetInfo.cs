using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Server
{
    public class TestGetInfo : TestServerBase
    {
        [Fact]
        public async Task GetChatroomInfo()
        {
            var info = await server.GetChatroomInfoAsync(1, 1);
            Assert.Equal(1, info.Id);
            Assert.Equal("Global Chatroom", info.Name);
            Assert.Equal(new HashSet<long> {1, 2}, info.PeopleIds.ToHashSet());
        }

        [Fact]
        public async Task GetChatroomInfo_NotInside()
        {
            var room = await server.NewChatroomAsync(new long[] {2});
            var info = await server.GetChatroomInfoAsync(1, room.Id);
            Assert.Equal(room.Id, info.Id);
            Assert.Equal("", info.Name);
            Assert.Equal(0, info.PeopleIds.Count);
        }

        [Fact]
        public async Task GetPeopleInfo()
        {
            var info = await server.GetPeopleInfoAsync(1, 2);
            Assert.Equal(2, info.Id);
            Assert.Equal("user2", info.Username);
        }
    }
}