using System;
using System.Collections.Generic;
using Chat.Server.Infrastructure;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("add")]
    class ChatroomAddPeopleOption : OptionBase
    {
        [Value(0)]
        public long ChatroomId { get; set; }
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }

        internal override void Execute(Program app)
        {
            foreach (var peopleId in PeopleIds)
            {
                app.server.AddPeopleToChatroom(ChatroomId, peopleId).Wait();
            }
            Console.WriteLine($"People {PeopleIds.ToJsonString()} added to chatroom {ChatroomId}.");
        }
    }
}
