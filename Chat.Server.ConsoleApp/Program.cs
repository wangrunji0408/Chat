using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CommandLine;
using NLog.Extensions.Logging;

using Chat.Server.ConsoleApp.Options;
using Chat.Connection.Grpc;
using Chat.Core.Models;
using System.Reflection;

namespace Chat.Server.ConsoleApp
{
    class Program
    {
        internal const string NlogConfigFile = "nlog.config";

        internal ConsoleOption consoleOption;
        internal Server server;
        internal ILogger _logger;
        internal ILogger _cmdlogger;
	    
	    internal static readonly Dictionary<string, TypeInfo[]> optionsDict;
	    static Program()
	    {
		    var groups = from type in Assembly.GetExecutingAssembly().DefinedTypes
			    let command = type.GetCustomAttribute<CommandAttribute>()?.Name
			    where command != null
			    group type by command;

		    optionsDict = groups.ToDictionary(g => g.Key, g => g.ToArray());
	    }
	    
	    void ParseFailed(IEnumerable<Error> obj)
	    {
		    Console.Error.WriteLine("Error:");
		    foreach (var e in obj)
			    Console.Error.WriteLine(e);
	    }

	    void ReadCommands()
	    {
		    while (true)
		    {
			    Console.Write("> ");
			    try
			    {
				    var cmd = Console.ReadLine();
				    if (string.IsNullOrWhiteSpace(cmd))
					    continue;
				    _cmdlogger.LogTrace(cmd);
				    var args = cmd.Split(' ');
				    var command = args[0];
				    var restArgs = args.Skip(1);

				    if(command == "help")
				    {
					    new HelpOption().Execute(this);
					    continue;
				    }
				    var parseResult = optionsDict.ContainsKey(command)?
					    Parser.Default.ParseArguments(restArgs, optionsDict[command]):
					    Parser.Default.ParseArguments(args, optionsDict[""]);
				    parseResult.WithParsed<OptionBase>(opt => opt.Execute(this))
					    .WithNotParsed(ParseFailed);
			    }
			    catch (Exception e)
			    {
				    Console.Error.WriteLine("Server throws an exception. Check 'console-exception.log' for details.");
				    Console.Error.WriteLine(e.Message);
				    _logger.LogError(e, "Server throws an exception.");
			    }
		    }
	    }
	   
	    static void Main(string[] args)
		{
			var app = new Program();
			Parser.Default
				  .ParseArguments<ConsoleOption>(args)
                  .WithParsed(opt => opt.Execute(app));
			app.ReadCommands();
		}
    }
}
