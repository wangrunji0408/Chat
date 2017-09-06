using CommandLine;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("user")]
    [Verb("friend")]
    class UserFriendOption: OptionBase
    {
        [Value(0)]
        public long User1Id { get; set; }
        [Value(1)]
        public long User2Id { get; set; }

        internal override void Execute(Program app)
        {
            app.server.MakeFriends(User1Id, User2Id);
        }
    }
}