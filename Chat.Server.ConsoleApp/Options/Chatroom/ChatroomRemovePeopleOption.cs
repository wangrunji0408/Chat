using System;
using System.Collections.Generic;
using Chat.Server.Infrastructure;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("remove")]
    class ChatroomRemovePeopleOption : OptionBase
    {
        [Value(0)]
        public long ChatroomId { get; set; }
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }

        internal override void Execute(Program app)
        {
            foreach (var peopleId in PeopleIds)
            {
                app.server.RemovePeopleFromChatroom(ChatroomId, peopleId).Wait();
            }
            Console.WriteLine($"People {PeopleIds.ToJsonString()} removed from chatroom {ChatroomId}.");
        }
    }
}
