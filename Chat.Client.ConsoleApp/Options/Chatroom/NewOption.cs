using System;
using System.Collections.Generic;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("new")]
    class NewOption: RoomOptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }
        [Option('n', "name", Default = "")]
        public string Name { get; set; }
        
        internal override void Execute(Program app)
        {
            var id = Chatroom.NewChatroom(PeopleIds, Name).Result;
            Console.WriteLine($"Success to create chatroom. Id = {id}.");
        }
    }
}