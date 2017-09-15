using CommandLine;

namespace Chat.Server.ConsoleApp.Options.User
{
    [Command("user")]
    [Verb("delete")]
    class DeleteOption : OptionBase
    {
        [Value(0)]
        public long UserId { get; set; }
    }
}
