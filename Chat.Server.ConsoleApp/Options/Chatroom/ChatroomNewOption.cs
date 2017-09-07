using System;
using System.Collections.Generic;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("new")]
    class ChatroomNewOption : OptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }

        internal override void Execute(Program app)
        {
            var chatroom = app.server.NewChatroomAsync(PeopleIds).Result;
            Console.WriteLine($"New chatroom {chatroom.Id}.");
        }
    }
}
