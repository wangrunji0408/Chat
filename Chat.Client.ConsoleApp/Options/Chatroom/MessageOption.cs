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
            var messages = Chatroom.GetRecentMessagesAsync(Count).Result;
            messages.ForEach(e => Console.WriteLine(e.ToReadString()));
        }
    }
}