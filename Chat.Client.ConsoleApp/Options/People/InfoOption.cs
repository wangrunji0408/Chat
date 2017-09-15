using System;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options.People
{
    [Verb("info")]
    class InfoOption: PeopleOptionBase
    {
        internal override void Execute(Program app)
        {
            var info = app.Client.GetPeopleInfo(PeopleId).Result;
            Console.WriteLine(info);
        }
    }
}