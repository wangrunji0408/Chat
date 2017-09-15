using System.Collections.Generic;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("remove")]
    class RemoveOption: RoomOptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }
        
        internal override void Execute(Program app)
        {
            Chatroom.RemovePeoplesAsync(PeopleIds).Wait();
        }
    }
}