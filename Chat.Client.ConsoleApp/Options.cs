using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Chat.Client.ConsoleApp.Options
{
    class ConsoleOption
    {
        [Option('s', "server_address", Required = true)]
        public string ServerAddress { get; set; }

		[Option('h', "host", Default = "localhost", HelpText = "Grpc service host.")]
		public string Host { get; set; }

        [Option('p', "port", HelpText = "Client service port.")]
        public int Port { get; set; }

		[Usage(ApplicationAlias = "dotnet run")]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Start the client, with a server in localhost",
                                         new ConsoleOption { ServerAddress = "localhost:8080" });
				yield return new Example("Start the client, with a server in LAN",
										 new ConsoleOption { ServerAddress = "192.168.1.10:8080", Host = "192.168.1.2" });
			}
		}
    }


	[Verb("signup")]
	class SignupOption
	{
        [Value(0, MetaName = "Username")]
		public string Username { get; set; }
        [Value(1, MetaName = "Password")]
		public string Password { get; set; }
	}

    [Verb("login")]
    class LoginOption
    {
        [Value(0, MetaName = "UserID")]
        public long UserId { get; set; }
        [Value(1, MetaName = "Password")]
        public string Password { get; set; }
    }

    [Verb("send")]
    class SendOption
    {
        [Value(0, MetaName = "Text")]
        public string Text { get; set; }
    }
}
