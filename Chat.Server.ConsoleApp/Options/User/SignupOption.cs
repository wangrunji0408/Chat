using Chat.Core.Models;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.User
{
    [Command("user")]
    [Verb("signup")]
    class SignupOption : OptionBase
    {
        [Value(0)]
        public string Username { get; set; }
        [Value(1)]
        public string Password { get; set; }

        internal override void Execute(Program app)
        {
			var request = new SignupRequest
			{
				Username = Username,
				Password = Password
			};
			var response = app.server.SignupAsync(request);
        }
    }
}
