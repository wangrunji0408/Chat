using System;
using System.Collections.Generic;
using Chat.Core.Models;
using CommandLine;
using CommandLine.Text;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("user")]
    [Verb("signup")]
    class UserSignupOption : OptionBase
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
			var response = app.server.Signup(request);
        }
    }
}
