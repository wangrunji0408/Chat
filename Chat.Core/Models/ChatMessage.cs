using System;
namespace Chat.Core.Models
{
    public partial class ChatMessage
    {
        public DateTimeOffset CreateTime => DateTimeOffset.FromUnixTimeSeconds(timeUnix_);
    }
}
