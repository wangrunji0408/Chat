using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Xunit;

namespace Chat.Test.Server
{
    public class TestChatroom : TestServerBase
    {
        private class MockClientService : IClientService
        {
            internal ChatMessage ReceivedMessage { get; private set; }

            public Task InformNewMessageAsync(ChatMessage message)
            {
                ReceivedMessage = message;
                return Task.CompletedTask;
            }

            public Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public async Task MessageForward()
        {
            var cs1 = new MockClientService();
            var cs2 = new MockClientService();
            server.SetUserClient(1, cs1);
            server.SetUserClient(2, cs2);

            await server.ReceiveNewMessageAsync(new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 1,
                Content = new Content {Text = "Message"}
            });

            foreach (var cs in new[] {cs1, cs2})
            {
                Assert.NotNull(cs.ReceivedMessage);
                Assert.Equal("Message", cs.ReceivedMessage.Content.Text);
                Assert.Equal(1, cs.ReceivedMessage.ChatroomId);
            }
        }

        [Fact]
        public async Task NewChatroom()
        {
            var t0 = DateTimeOffset.Now;
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var t1 = DateTimeOffset.Now;

            Assert.Equal(2, room.Id);
            Assert.Equal(0, room.CreatorId);
            Assert.Equal(new HashSet<long> {1, 2}, room.UserIds.ToHashSet());
            Assert.True(room.CreateTime > t0 && room.CreateTime < t1);

            var user1 = await server.FindUserAsync(1);
            var user2 = await server.FindUserAsync(2);
            Assert.Contains(room.Id, user1.ChatroomIds);
            Assert.Contains(room.Id, user2.ChatroomIds);
        }
        
        [Fact]
        public async Task NewChatroomByUser()
        {
            await server.MakeFriendsAsync(1, 2);
            var ca = await server.GetChatroomApplication(0, 1)
                                 .NewChatroomAsync(new long[] {1, 2});
            var room = await server.FindChatroomAsync(ca.ChatroomId);

            Assert.Equal(1, room.CreatorId);
            Assert.Equal(UserChatroom.UserRole.Admin, room.GetUserChatroom(1).Role);
            Assert.Equal(UserChatroom.UserRole.Normal, room.GetUserChatroom(2).Role);
        }
        
        [Fact]
        public async Task NewChatroomByUser_NotFriend()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await server.GetChatroomApplication(0, 1)
                                        .NewChatroomAsync(new long[] {1, 2}));
        }
        
        [Fact]
        public async Task NewChatroomByUser_DoesNotContainSelf()
        {
            await server.MakeFriendsAsync(1, 2);
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await server.GetChatroomApplication(0, 1)
                    .NewChatroomAsync(new long[] {2}));
        }

        [Fact]
        public async Task AddPeople()
        {
            var room = await server.NewChatroomAsync(new long[] {1});
            var ca = server.GetChatroomApplication(room.Id, 0);
            Assert.DoesNotContain(2, room.UserIds);
            await ca.AddPeopleAsync(userId: 2);
            Assert.Contains(2, room.UserIds);
            
            var user2 = await server.FindUserAsync(2);
            Assert.Contains(room.Id, user2.ChatroomIds);
        }
        
        [Fact]
        public async Task RemovePeople()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 0);
            var user1 = await server.FindUserAsync(1);
            
            Assert.Contains(1, room.UserIds);
            await ca.RemovePeopleAsync(userId: 1);
            Assert.DoesNotContain(1, room.UserIds);
            Assert.DoesNotContain(room.Id, user1.ChatroomIds);
        }
        
        [Fact]
        public async Task NewP2PChatroom()
        {
            var room = await server.GetP2PChatroom(1, 2);
            Assert.Contains(1, room.UserIds);
            Assert.Contains(2, room.UserIds);
            Assert.True(room.IsP2P);
        }
        
        [Fact]
        public async Task ThrowWhenTryToChangePeopleInP2P()
        {
            var room = await server.GetP2PChatroom(1, 2);
            var ca = server.GetChatroomApplication(room.Id, 0);

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.AddPeopleAsync(userId: 3));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.RemovePeopleAsync(userId: 1));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.RemovePeopleAsync(userId: 2));
        }
        
        [Fact]
        public async Task Dismiss()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 0);

            await ca.DismissAsync();
            Assert.False(room.IsActive);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.AddPeopleAsync(userId: 3));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.RemovePeopleAsync(userId: 1));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.RemovePeopleAsync(userId: 2));
        }

        [Fact]
        public async Task ThrowWhenPermissionDenied()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 1);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.AddPeopleAsync(userId: 3));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.RemovePeopleAsync(userId: 2));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.DismissAsync());
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await ca.ChangeNameAsync("haha"));
            await server.GetChatroomApplication(room.Id, 0).SetRoleAsync(1, "Admin");
            await ca.AddPeopleAsync(userId: 3);
            await ca.RemovePeopleAsync(userId: 2);
            await ca.ChangeNameAsync("haha");
            await ca.DismissAsync();
        }
        
        [Fact]
        public async Task SetRole()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 0);

            Assert.Equal(UserChatroom.UserRole.Normal, room.GetUserChatroom(1).Role);
            Assert.Equal(UserChatroom.UserRole.Normal, room.GetUserChatroom(2).Role);
            await ca.SetRoleAsync(1, "Admin");
            Assert.Equal(UserChatroom.UserRole.Admin, room.GetUserChatroom(1).Role);
            Assert.Equal(UserChatroom.UserRole.Normal, room.GetUserChatroom(2).Role);
            
        }
        
        [Fact]
        public async Task Quit()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 1);
            var user = await server.FindUserAsync(1);
            Assert.Contains(room.Id, user.ChatroomIds);
            await ca.QuitAsync();
            Assert.DoesNotContain(room.Id, user.ChatroomIds);
        }
        
        [Fact]
        public async Task ChangeName()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            var ca = server.GetChatroomApplication(room.Id, 0);
            await ca.ChangeNameAsync("haha");
            Assert.Equal("haha", room.Name);
        }
    }
}