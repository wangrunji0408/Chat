﻿using CommandLine;
using System;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("room")]
    [Verb("info")]
    class ChatroomInfoOption : OptionBase
    {
        [Value(0)]
        public long? ChatroomId { get; set; }

        internal override void Execute(Program app)
        {
            if(ChatroomId == null)
            {
                var chatrooms = app.server.GetChatroomsStringAsync().Result;
                foreach (var c in chatrooms)
                    Console.WriteLine(c);
                return;
            }
            var chatroom = app.server.GetChatroomStringAsync(ChatroomId.Value).Result;
            if (chatroom == null)
            {
                Console.Error.WriteLine($"Chatroom {ChatroomId} does not exist.");
				return;
            }
            Console.WriteLine(chatroom);
        }
    }
}