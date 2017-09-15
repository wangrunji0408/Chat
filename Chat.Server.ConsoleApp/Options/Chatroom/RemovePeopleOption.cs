using System;
using System.Collections.Generic;
using Chat.Server.Infrastructure;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("remove")]
    class RemovePeopleOption : OptionBase
    {
        [Value(0)]
        public long ChatroomId { get; set; }
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }

        internal override void Execute(Program app)
        {
            app.server.GetChatroomApplication(ChatroomId, 0)
                .RemovePeoplesAsync(PeopleIds).Wait();
            Console.WriteLine($"People {PeopleIds.ToJsonString()} removed from chatroom {ChatroomId}.");
        }
    }
}
