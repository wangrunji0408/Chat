using System;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("info")]
    class InfoOption: RoomOptionBase
    {
        internal override void Execute(Program app)
        {
            var info = Chatroom.GetInfoAsync().Result;
            Console.WriteLine(info);
        }
    }
}