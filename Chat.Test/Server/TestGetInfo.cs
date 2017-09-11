using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
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
            Assert.Equal(new long[]{1, 2, 3}, info.PeopleIds);
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
        public async Task GetChatroomInfo_P2P()
        {
            var room = await server.GetP2PChatroom(1, 2);
            var info = await server.GetChatroomInfoAsync(1, room.Id);
            Assert.Equal(room.Id, info.Id);
            Assert.Equal("", info.Name);
            Assert.True(info.IsP2P);
            Assert.Equal(new long[]{1, 2}, info.PeopleIds);
        }

        [Fact]
        public async Task GetPeopleInfo_Stranger()
        {
            var user = await server.FindUserAsync(2);
            var info = await server.GetPeopleInfoAsync(1, 2);
            AssertGetPeopleInfoBasic(user, info);
            Assert.Equal(PeopleInfo.RoleInfoOneofCase.Stranger, info.RoleInfoCase);
        }
        
        [Fact]
        public async Task GetPeopleInfo_Friend()
        {
            await server.MakeFriends(1, 2);
            var user = await server.FindUserAsync(2);
            var info = await server.GetPeopleInfoAsync(1, 2);
            AssertGetPeopleInfoBasic(user, info);
            Assert.Equal(PeopleInfo.RoleInfoOneofCase.Friend, info.RoleInfoCase);
        }
        
        [Fact]
        public async Task GetPeopleInfo_Self()
        {
            var user = await server.FindUserAsync(1);
            var info = await server.GetPeopleInfoAsync(1, 1);
            AssertGetPeopleInfoBasic(user, info);
            Assert.Equal(PeopleInfo.RoleInfoOneofCase.Self, info.RoleInfoCase);
            Assert.Equal(user.CreateTime.ToUnixTimeMilliseconds(), info.Self.SignupTimeUnixMs);
        }
        
        private void AssertGetPeopleInfoBasic(User user, PeopleInfo info)
        {
            Assert.Equal(user.Id, info.Id);
            Assert.Equal(user.Username, info.Username);
        }
    }
}