using System;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Database
{
    [Command("db")]
    [Verb("clear")]
    class DbClearOption: OptionBase
    {
        internal override void Execute(Program app)
        {
            Console.Write("Are you sure to clear database? y/[n]: ");
            var input = Console.ReadLine();
            if(input.ToLower() == "y")
            {
                app.server.ClearDatabase();
                Console.WriteLine("Database cleared.");
            }
        }
    }
}