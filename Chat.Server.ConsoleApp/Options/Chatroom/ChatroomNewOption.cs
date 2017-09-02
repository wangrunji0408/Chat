using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Chat.Server.ConsoleApp.Options
{

    [Command("chatroom")]
    [Verb("new")]
    class ChatroomNewOption : OptionBase
    {
        [Option('p', "peoples", Separator = ',')]
        public IEnumerable<long> PeopleIds { get; set; }
    }
}
