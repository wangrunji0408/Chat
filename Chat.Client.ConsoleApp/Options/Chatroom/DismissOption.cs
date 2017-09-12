using System.Collections.Generic;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("dismiss")]
    class DismissOption: RoomOptionBase
    {        
        internal override void Execute(Program app)
        {
            Chatroom.DismissAsync().Wait();
        }
    }
}