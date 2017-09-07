using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;
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
            Assert.Equal(new HashSet<long> {1, 2}, room.UserIds.ToHashSet());
            Assert.True(room.CreateTime > t0 && room.CreateTime < t1);
        }

        [Fact]
        public async Task NewPeople()
        {
            var room = await server.NewChatroomAsync(new long[] {1});
            Assert.DoesNotContain(2, room.UserIds);
            await server.AddPeopleToChatroom(room.Id, userId: 2);
            Assert.Contains(2, room.UserIds);
        }
        
        [Fact]
        public async Task RemovePeople()
        {
            var room = await server.NewChatroomAsync(new long[] {1, 2});
            Assert.Contains(1, room.UserIds);
            await server.RemovePeopleFromChatroom(room.Id, userId: 1);
            Assert.DoesNotContain(1, room.UserIds);
        }
    }
}