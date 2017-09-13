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
            var e = new NewChatroomEvent{ChatroomId = 2, OperatorId = 1};
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                ChatroomId = 2,
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                Content = new Content{Created = new Content.Types.Created{CreatorId = 1}}
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
            var e = new NewMessageEvent{ChatroomId = 2, OperatorId = 1, Message = message};
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(message, m);
        }
        
        [Fact]
        public void MapUserEnteredChatroomEvent()
        {
            var e = new UserEnteredEvent{ChatroomId = 1, UserId = 2, OperatorId = 0};
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
            var e = new UserLeftEvent{ChatroomId = 1, UserId = 2, OperatorId = 0};
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                SenderId = 0,
                ChatroomId = 1,
                Content = new Content{PeopleLeave = new Content.Types.PeopleLeave{PeopleId = 2}}
            }, m);
        }
        
        [Fact]
        public void MapPropertyChangedEvent()
        {
            var e = new PropertyChangedEvent
            {
                ChatroomId = 1, 
                OperatorId = 2, 
                Key = "Name",
                Value = "New Chatroom",
            };
            var m = ChatroomEventMapper.Map(e);
            Assert.Equal(new ChatMessage
            {
                TimeUnixMs = e.Time.ToUnixTimeMilliseconds(),
                SenderId = 0,
                ChatroomId = 1,
                Content = new Content{SetPeoperty = new Content.Types.SetProperty
                {
                    Key = "Name",
                    Value = "New Chatroom"
                }}
            }, m);
        }
    }
}