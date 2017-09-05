using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Chat.Server.ConsoleApp.Options
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
