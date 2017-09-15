using System;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
	[Command("room")]
	[Verb("message")]
	class MessageOption : OptionBase
	{
		[Value(0)]
		public long ChatroomId { get; set; }
		[Option('c', "count", Default = 10)]
		public int Count { get; set; }

		internal override void Execute(Program app)
		{
            var messages = app.server.GetRecentMessagesAsync(ChatroomId, Count).Result;
			messages.ForEach(Console.WriteLine);
		}
	}
}
