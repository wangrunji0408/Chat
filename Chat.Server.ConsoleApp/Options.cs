using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Chat.Server.ConsoleApp.Options
{
    class ConsoleOption
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
    }

	[Verb("signup")]
	class SignupOption
	{
        [Value(0)]
		public string Username { get; set; }
        [Value(1)]
		public string Password { get; set; }
	}

	[Verb("delete")]
	class DeleteOption
	{
		[Value(0)]
		public long UserId { get; set; }
	}

    [Verb("userinfo")]
    class UserInfoOption
	{
		[Value(0)]
		public long UserId { get; set; }
	}

	[Verb("message")]
	class MessageOption
	{
		[Value(0)]
		public int Count { get; set; }
	}

	[Verb("database_clear")]
	class DatabaseClearOption
	{
	}
}
