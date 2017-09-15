using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options.Chatroom
{
    [Verb("room")]
    class RoomOption: RootOptionBase
    {
        [Value(0, Required = true)]
        public long ChatroomId { get; set; }
        
        protected override string Str => $"Room {ChatroomId}";
        protected override Type BaseOptionType => typeof(RoomOptionBase);
        protected override void AfterParsed(OptionBase opt, Program app)
        {
            base.AfterParsed(opt, app);
            var chatroom = app.Client.GetChatroom(ChatroomId);
            var ropt = (RoomOptionBase) opt;
            ropt.ChatroomId = ChatroomId;
            ropt.Chatroom = chatroom;
        }

        internal override void Execute(Program app)
        {
            ReadCommands(app);
        }
    }

    abstract class RoomOptionBase : OptionBase
    {
        public long ChatroomId { get; set; }
        public Chat.Client.Chatroom Chatroom { get; set; }
    }
}