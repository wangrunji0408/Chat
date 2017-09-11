using System;
using Chat.Core.Models;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Services;
using Xunit;

namespace Chat.Test.Server.Internal
{
    public class TestMapChatroomEvent
    {
        [Fact]
        public void MapNewChatroomEvent()
        {
            var e = new NewChatroomEvent(1);
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                ChatroomId = 1,
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                Content = new Content{Created = new Content.Types.Created()}
            }, m);
        }
        
        [Fact]
        public void MapNewMessageEvent()
        {
            var message = new ChatMessage
            {
                ChatroomId = 2,
                SenderId = 1,
                TimeUnixMs = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Content = new Content{Text = "Hello!"}
            };
            var e = new NewMessageEvent(message);
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(message, m);
        }
        
        [Fact]
        public void MapUserEnteredChatroomEvent()
        {
            var e = new UserEnteredChatroomEvent(chatroomId: 1, userId: 2);
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                SenderId = 0,
                ChatroomId = 1,
                Content = new Content{PeopleEnter = new Content.Types.PeopleEnter{PeopleId = 2}}
            }, m);
        }
        
        [Fact]
        public void MapUserLeftChatroomEvent()
        {
            var e = new UserLeftChatroomEvent(chatroomId: 1, userId: 2);
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                SenderId = 0,
                ChatroomId = 1,
                Content = new Content{PeopleLeave = new Content.Types.PeopleLeave{PeopleId = 2}}
            }, m);
        }
    }
}