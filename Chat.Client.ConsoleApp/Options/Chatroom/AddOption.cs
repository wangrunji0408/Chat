using System.Collections.Generic;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("add")]
    class AddOption: RoomOptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }
        
        internal override void Execute(Program app)
        {
            Chatroom.AddPeoplesAsync(PeopleIds).Wait();
        }
    }
}