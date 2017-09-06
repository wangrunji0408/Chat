using Chat.Core.Models;

namespace Chat.Server.Domains.Events
{
    class NewMessageEvent: ChatroomEvent
    {
        public ChatMessage Message { get; set; }
    }
}