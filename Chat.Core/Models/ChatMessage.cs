using System;
using Google.Protobuf;

namespace Chat.Core.Models
{
    public partial class ChatMessage
    {
        public DateTimeOffset CreateTime => DateTimeOffset.FromUnixTimeSeconds(timeUnix_);

        public string ContentString
        {
            get => JsonFormatter.Default.Format(Content);
            set => Content = JsonParser.Default.Parse<Content>(value);
        }
    }
}
