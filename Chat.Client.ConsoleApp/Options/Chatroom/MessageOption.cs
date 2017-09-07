using System;
using System.Linq;
using Chat.Core.Models;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("message")]
    class MessageOption: RoomOptionBase
    {
        [Option('c', "count", Default = 10)]
        public int Count { get; set; }
        
        internal override void Execute(Program app)
        {
            var request = new GetMessagesRequest
            {
                ChatroomId = ChatroomId,
                AfterTimeUnixMs = 0,
                Count = Count
            };
            var messages = app.Client.GetMessages(request).Result;
            messages = messages.OrderBy(m => m.TimeUnixMs).ToList();
            messages.ForEach(e => Console.WriteLine(e.ToReadString()));
        }
    }
}