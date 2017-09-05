using CommandLine;

namespace Chat.Client.ConsoleApp.Options
{
    [Verb("send")]
    class SendOption: RoomOptionBase
    {
        [Value(0, MetaName = "Text")]
        public string Text { get; set; }

        internal override void Execute(Program app)
        {
            app.Client.SendTextMessage(Text, ChatroomId).Wait();
        }
    }
}