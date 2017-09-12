using System;
using System.Collections.Generic;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("new")]
    class NewOption : OptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }
        [Option('n', "name", Default = "")]
        public string Name { get; set; }

        internal override void Execute(Program app)
        {
            var id = app.server.GetChatroomApplication(roomId: 0, operatorId: 0)
                .NewChatroomAsync(PeopleIds, Name).Result;
            Console.WriteLine($"New chatroom {id}.");
        }
    }
}
