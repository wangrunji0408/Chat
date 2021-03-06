﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chat.Connection.Grpc;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Chat.Server.ConsoleApp.Options
{
    class ConsoleOption: OptionBase
    {
        [Option('h', "host", Default = "localhost", HelpText = "Grpc service host.")]
        public string Host { get; set; }
        [Option('p', "port", HelpText = "Grpc service port.")]
        public int Port { get; set; }
        [Option('d', "database", Default = DbType.InMemory, HelpText = "Database type.")]
        public DbType Database { get; set; }
        [Option("sqlite", Default = "DataSource=data.db", HelpText = "SQLite connection string.")]
        public string SQLiteString { get; set; }

        public enum DbType
        {
            InMemory, SQLite
        }

		[Usage(ApplicationAlias = "dotnet run")]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Start the server at localhost", 
                                         new ConsoleOption());
				yield return new Example("Start the server in LAN. 'host' is the IP of this computer",
                                         new ConsoleOption{Host="192.168.1.10", Port=8080});
			}
		}

        internal override void Execute(Program app)
        {
	        var lf = new LoggerFactory();
	        lf.AddNLog().ConfigureNLog(Program.NlogConfigFile);
	        app._logger = lf.CreateLogger("Chat.Server.Console");
	        app._cmdlogger = lf.CreateLogger("Chat.Server.Console.Commands");
	        GrpcConnectionExtension.SetLogger(lf);
	        
	        app.consoleOption = this;
	        var builder = new ServerBuilder()
		        .UseLoggerFactory(lf)
		        .UseGrpc(Host, Port);
	        if (Database == DbType.InMemory)
		        builder.UseInMemory();
	        else if (Database == DbType.SQLite)
		        builder.UseSQLite(SQLiteString);
	        app.server = builder.Build();
        }
    }
}
