using System;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options
{
    [Verb("room")]
    class RoomOption: OptionBase
    {
        [Value(0, Required = true)]
        public long ChatroomId { get; set; }
        
        internal override void Execute(Program app)
        {
            while(true)
            {
                Console.Write($"Room {ChatroomId} > ");
                try
                {
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd))
                        continue;
                    app.Cmdlogger.LogTrace(cmd);
                    var args = cmd.Split(' ');

                    if (args[0] == "q" || args[0] == "exit")
                        return;
                    
                    Parser.Default.ParseArguments<SendOption, InfoOption>(args)
                        .WithParsed<RoomOptionBase>(opt =>
                        {
                            opt.ChatroomId = ChatroomId;
                            opt.Execute(app);
                        })
                        .WithNotParsed(app.ParseFailed);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
    }

    abstract class RoomOptionBase : OptionBase
    {
        public long ChatroomId { get; set; }
    }
}