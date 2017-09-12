using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Logging;
using Chat.Client.ConsoleApp.Options.People;

namespace Chat.Client.ConsoleApp.Options
{
    [Verb("people")]
    class PeopleOption: OptionBase
    {
        [Value(0, Required = true)]
        public long PeopleId { get; set; }
        
        internal override void Execute(Program app)
        {
            var suboptions = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => t.IsSubclassOf(typeof(PeopleOptionBase)))
                .ToArray();
            
            while(true)
            {
                Console.Write($"People {PeopleId} > ");
                try
                {
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd))
                        continue;
                    app.Cmdlogger.LogTrace(cmd);
                    var args = cmd.Split(' ');

                    if (args[0] == "q" || args[0] == "exit")
                        return;
                    
                    Parser.Default.ParseArguments(args, suboptions)
                        .WithParsed<PeopleOptionBase>(opt =>
                        {
                            opt.PeopleId = PeopleId;
                            opt.Execute(app);
                        })
                        .WithNotParsed(app.ParseFailed);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Client throws an exception. Check 'console-exception.log' for details.");
                    Console.Error.WriteLine(e.Message);
                    app.Logger.LogError(e, "Client throws an exception.");
                }
            }
        }
    }

    abstract class PeopleOptionBase : OptionBase
    {
        public long PeopleId { get; set; }
    }
}