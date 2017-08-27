using System;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options
{
    class ConsoleOption
    {
        [Option('p', "port", HelpText = "Grpc service port.")]
        public int Port { get; set; }
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
}
