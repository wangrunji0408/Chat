using System;
using Chat.Core.Models;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.People
{
    [Verb("makefriend")]
    class MakeFriendOption: PeopleOptionBase
    {
        [Option('g')]
        public string Greeting { get; set; }
        
        internal override void Execute(Program app)
        {
            var response = app.Client.MakeFriend(PeopleId, Greeting).Result;
            if(response.Status == MakeFriendResponse.Types.Status.Accept)
                Console.WriteLine("Accepted.");
            else
                Console.WriteLine($"Failed: [{response.Status}] {response.Detail}");
        }
    }
}