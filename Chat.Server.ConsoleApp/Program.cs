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

        //void ClearDatabase (DatabaseClearOption opt)
        //{
        //    Console.Write("Are you sure to clear database? y/[n]: ");
        //    var input = Console.ReadLine();
        //    if(input.ToLower() == "y")
        //    {
        //        server.ClearDatabase();
        //        Console.WriteLine("Database cleared.");
        //    }
        //}

	    static void Main(string[] args)
		{
			var app = new Program();
			Parser.Default
				  .ParseArguments<ConsoleOption>(args)
                  .WithParsed(opt => opt.Execute(app));
		}
    }
}
