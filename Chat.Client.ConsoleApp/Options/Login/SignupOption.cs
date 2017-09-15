using System;
using CommandLine;

namespace Chat.Client.ConsoleApp.Options
{
	[Verb("signup")]
	class SignupOption: RootOptionBase
	{
        [Value(0, MetaName = "Username")]
		public string Username { get; set; }
        [Value(1, MetaName = "Password")]
		public string Password { get; set; }

		internal override void Execute(Program app)
		{
			app.Builder.SignupAsync(Username, Password).Wait();
		}
	}
}
