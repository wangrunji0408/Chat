using System;
using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
	[Command("room")]
	[Verb("message")]
	class ChatroomMessageOption : OptionBase
	{
		[Value(0)]
		public long ChatroomId { get; set; }
		[Value(1)]
		public int Count { get; set; }

		internal override void Execute(Program app)
		{
            var messages = app.server.GetRecentMessages(ChatroomId, Count).Result;
			messages.ForEach(Console.WriteLine);
		}
	}
}
