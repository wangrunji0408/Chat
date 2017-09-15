using System;
using Chat.Core.Models;

namespace Chat.Server.Domains.Events.Chatroom
{
    abstract class ChatroomEvent: DomainEvent
    {
        public long ChatroomId { get; set; }
        public long OperatorId { get; set; }
    }
    abstract class ChatroomUserEvent : ChatroomEvent
    {
        public long UserId { get; set; }
    }
    class DismissedEvent: ChatroomEvent
    {
    } 
    class NewChatroomEvent: ChatroomEvent
    {
    } 
    class NewMessageEvent: ChatroomEvent
    {
        public ChatMessage Message { get; set; }
    } 
    class PropertyChangedEvent : ChatroomEvent
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    class UserEnteredEvent: ChatroomUserEvent
    {
    } 
    class UserLeftEvent: ChatroomUserEvent
    {
    }
    class SetRoleEvent : ChatroomUserEvent
    {
        public string NewRole { get; set; }
    }
    class UserBlockedEvent : ChatroomUserEvent
    {
    }
}