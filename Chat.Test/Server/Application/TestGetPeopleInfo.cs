using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Xunit;

namespace Chat.Test.Server
{
    public class TestGetPeopleInfo : TestServerBase
    {
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
            await server.MakeFriendsAsync(1, 2);
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