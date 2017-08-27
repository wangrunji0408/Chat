using System;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options
{
    class ConsoleOption
    {
        [Option('s', "server_address", Required = true)]
        public string ServerAddress { get; set; }

        [Option('p', "port", HelpText = "Client service port.")]
        public int Port { get; set; }
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
