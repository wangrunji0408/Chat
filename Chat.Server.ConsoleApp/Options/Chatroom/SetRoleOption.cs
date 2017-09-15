using CommandLine;

namespace Chat.Server.ConsoleApp.Options.Chatroom
{
    [Command("room")]
    [Verb("setrole")]
    class SetRoleOption: OptionBase
    {
        [Value(0)]
        public long ChatroomId { get; set; }
        [Value(1)]
        public long UserId { get; set; }
        [Value(2)]
        public string Role { get; set; }

        internal override void Execute(Program app)
        {
            app.server.GetChatroomApplication(ChatroomId, 0)
                .SetRoleAsync(UserId, Role).Wait();
        }
    }
}