using System;
using System.Linq;
using System.Reflection;
using Chat.Client.ConsoleApp.Options.Chatroom;
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
            var suboptions = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => t.IsSubclassOf(typeof(RoomOptionBase)))
                .ToArray();
            
            var chatroom = app.Client.GetChatroom(ChatroomId);
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
                    
                    Parser.Default.ParseArguments(args, suboptions)
                        .WithParsed<RoomOptionBase>(opt =>
                        {
                            opt.ChatroomId = ChatroomId;
                            opt.Chatroom = chatroom;
                            opt.Execute(app);
                        })
                        .WithNotParsed(app.ParseFailed);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Client throws an exception. Check 'console-exception.log' for details.");
                    Console.Error.WriteLine(e.Message);
                    app.Logger.LogError(e, "Client throws an exception.");
                }
            }
        }
    }

    abstract class RoomOptionBase : OptionBase
    {
        public long ChatroomId { get; set; }
        public Chat.Client.Chatroom Chatroom { get; set; }
    }
}