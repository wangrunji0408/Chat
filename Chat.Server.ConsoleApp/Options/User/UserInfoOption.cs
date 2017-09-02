using System;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("user")]
    [Verb("info")]
    class UserInfoOption : OptionBase
    {
        [Value(0)]
        public long UserId { get; set; }

        internal override void Execute(Program app)
        {
			var user = app.server.GetUserNullable(UserId);
			if (user == null)
			{
				Console.Error.WriteLine($"User {UserId} does not exist.");
				return;
			}
			Console.WriteLine(user);
        }
    }
}
