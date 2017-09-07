using System;

namespace Chat.Server.Domains.Events.Chatroom
{
    class UserLeftChatroomEvent: ChatroomEvent
    {
        public long UserId { get; protected set; }
        
        public UserLeftChatroomEvent(long chatroomId, long userId)
        {
            ChatroomId = chatroomId;
            UserId = userId;
        }
        
        public override string ToString()
        {
            return $"[{nameof(UserLeftChatroomEvent)} " +
                   $"{nameof(UserId)}: {UserId}, " +
                   $"{nameof(ChatroomId)}: {ChatroomId}]";
        }
    }
}