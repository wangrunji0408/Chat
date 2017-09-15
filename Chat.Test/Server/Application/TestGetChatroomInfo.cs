using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Server
{
    public class TestGetChatroomInfo: TestServerBase
    {
        public TestGetChatroomInfo()
        {
            CreateChatroom().Wait();
        }

        async Task CreateChatroom()
        {
            await server.SignupAsync(new SignupRequest
            {
                Username = "user4",
                Password = "password"
            });
            
            var room = await server.GetChatroomApplication(0, 0)
                .NewChatroomAsync(new long[] {1, 2, 3}, "Room");
            roomId = room.ChatroomId;
            await room.SetRoleAsync(1, "Admin");
            await room.BlockAsync(3);
        }

        private long roomId;
        
        [Fact]
        public async Task AdminView()
        {
            var info = await server.GetChatroomInfoAsync(1, roomId);
            Assert.Equal(roomId, info.Id);
            Assert.Equal("Room", info.Name);
            Assert.True(info.IsActive);
            Assert.False(info.IsP2P);
            Assert.Equal(new []
            {
                new ChatroomInfo.Types.PeopleInRoom {
                    Id = 1, Name = "", Role = "Admin", IsBlocked = false
                }, new ChatroomInfo.Types.PeopleInRoom {
                    Id = 2, Name = "", Role = "Normal", IsBlocked = false
                }, new ChatroomInfo.Types.PeopleInRoom {
                    Id = 3, Name = "", Role = "Normal", IsBlocked = true
                }, 
            }, info.Peoples);
        }
        
        [Fact]
        public async Task NormalView()
        {
            var info = await server.GetChatroomInfoAsync(2, roomId);
            Assert.Equal(roomId, info.Id);
            Assert.Equal("Room", info.Name);
            Assert.True(info.IsActive);
            Assert.False(info.IsP2P);
            Assert.Equal(new []
            {
                new ChatroomInfo.Types.PeopleInRoom {
                    Id = 1, Name = "", Role = "Admin", IsBlocked = false
                }, new ChatroomInfo.Types.PeopleInRoom {
                    Id = 2, Name = "", Role = "Normal", IsBlocked = false
                }, new ChatroomInfo.Types.PeopleInRoom {
                    Id = 3, Name = "", Role = "Normal", IsBlocked = true
                }, 
            }, info.Peoples);
        }

        [Fact]
        public async Task NotInside()
        {
            var info = await server.GetChatroomInfoAsync(4, roomId);
            Assert.Equal(roomId, info.Id);
            Assert.Equal("", info.Name);
            Assert.Equal(0, info.Peoples.Count);
            Assert.False(info.IsActive);
            Assert.False(info.IsP2P);
        }
        
        [Fact]
        public async Task P2P()
        {
            var room = await server.GetP2PChatroom(1, 2);
            var info = await server.GetChatroomInfoAsync(1, room.Id);
            Assert.Equal(room.Id, info.Id);
            Assert.Equal("", info.Name);
            Assert.True(info.IsP2P);
            Assert.True(info.IsActive);
            Assert.Equal(new long[]{1, 2}, info.PeopleIds);
        }
    }
}