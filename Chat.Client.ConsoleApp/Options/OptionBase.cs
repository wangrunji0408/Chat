using System;
using System.Linq;
using System.Reflection;
using Chat.Client.ConsoleApp.Options.Me;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options
{
    abstract class OptionBase
    {
        internal virtual void Execute(Program app)
        {
            throw new NotImplementedException();
        }

        protected virtual Type BaseOptionType => throw new NotImplementedException();
        protected virtual string Str => "";

        protected virtual void AfterParsed(OptionBase opt, Program app)
        {
        }
        
        protected void ReadCommands(Program app)
        {
            var suboptions = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => t.IsSubclassOf(BaseOptionType))
                .ToArray();
            
            while(true)
            {
                Console.Write($"{Str} > ");
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
                        .WithParsed<OptionBase>(opt =>
                        {
                            AfterParsed(opt, app);
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

    abstract class RootOptionBase: OptionBase
    {
    }
}
