using System;
using System.Reflection;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("help")]
    class HelpOption: OptionBase
    {
        //[Value(0)]
        //public string CommandName { get; set; }

        internal override void Execute(Program app)
        {
			Console.WriteLine("Commands:");
			foreach (var pair in ConsoleOption.optionsDict)
			{
				foreach (var type in pair.Value)
				{
					var verb = type.GetCustomAttribute<VerbAttribute>();
                    if (verb == null)
                    {
                        Console.WriteLine($"\t{pair.Key}");
                        continue;
                    }
					Console.WriteLine($"\t{pair.Key} {verb.Name}: {verb.HelpText}");
				}
			}
        }
    }
}
