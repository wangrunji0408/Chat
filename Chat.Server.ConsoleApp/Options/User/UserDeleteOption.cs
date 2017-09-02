using CommandLine;

namespace Chat.Server.ConsoleApp.Options
{
    [Command("user")]
    [Verb("delete")]
    class UserDeleteOption : OptionBase
    {
        [Value(0)]
        public long UserId { get; set; }
    }
}
