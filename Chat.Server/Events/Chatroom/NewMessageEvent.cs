using Chat.Core.Models;

namespace Chat.Server.Events
{
    class NewMessageEvent: ChatroomEvent
    {
        public ChatMessage Message { get; set; }
    }
}