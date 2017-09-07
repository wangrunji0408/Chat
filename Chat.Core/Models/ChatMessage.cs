using System;
using System.Text;
using Google.Protobuf;

namespace Chat.Core.Models
{
    public partial class ChatMessage
    {
        public DateTimeOffset CreateTime => DateTimeOffset.FromUnixTimeMilliseconds(timeUnixMs_);

        public string ContentString
        {
            get => JsonFormatter.Default.Format(Content);
            set => Content = JsonParser.Default.Parse<Content>(value);
        }

        public string ToReadString()
        {
            var builder = new StringBuilder();
            builder.Append(CreateTime);
            builder.Append($"[Room {ChatroomId}]");
            builder.Append(SenderId == 0 ? "[System]" : $"[User {SenderId}]");
            builder.Append(" ");

            switch (Content.ContentCase)
            {
                case Content.ContentOneofCase.Text:
                    builder.Append(Content.Text);
                    break;
                case Content.ContentOneofCase.PeopleEnter:
                    builder.Append($"User {Content.PeopleEnter.PeopleId} entered.");
                    break;
                case Content.ContentOneofCase.PeopleLeave:
                    builder.Append($"User {Content.PeopleLeave.PeopleId} left.");
                    break;
                default:
                    builder.Append(Content);
                    break;
            }
            return builder.ToString();
        }
    }
}
