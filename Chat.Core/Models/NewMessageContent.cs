using System;
namespace Chat.Core.Models
{
    public class NewMessageContent
    {
        public long SenderId { get; set; }
        public string Message { get; set; }
    }
}
