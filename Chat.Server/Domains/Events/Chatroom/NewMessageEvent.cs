using Chat.Core.Models;

namespace Chat.Server.Domains.Events.Chatroom
{
    class NewMessageEvent: ChatroomEvent
    {
        public ChatMessage Message { get; set; }

        public NewMessageEvent(ChatMessage message)
        {
            ChatroomId = message.ChatroomId;
            Message = message;
        }

        public override string ToString()
        {
            return $"[{nameof(NewMessageEvent)} {nameof(Message)}: {Message}]";
        }
    }
}