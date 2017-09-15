using System;
using System.Threading;
using System.Threading.Tasks;
using Chat.Client.ConsoleApp.Options.Chatroom;
using Chat.Client.ConsoleApp.Options.People;
using Chat.Connection.Grpc;
using Chat.Core.Models;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options
{
    [Verb("login")]
    class LoginOption: RootOptionBase
    {
        [Value(0, MetaName = "Username")]
        public string Username { get; set; }
        [Value(1, MetaName = "Password")]
        public string Password { get; set; }

        internal override void Execute(Program app)
        {
            try 
            {
                app.Client = app.Builder.LoginAsync(Username, Password).Result;
                ListenClientEvents(app.Client);
                ShowMessages(app.Client);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine($"Failed: {e.Message}");
                app.Client = null;
                return;
            }
            ReadCommands(app);
        }
        
        void ReadCommands(Program app)
        {
            while(true)
            {
                Console.Write($"{Username} > ");
                try
                {
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd))
                        continue;
                    app.Cmdlogger.LogTrace(cmd);
                    var args = cmd.Split(' ');
                    if (args[0] == "q" || args[0] == "exit")
                        return;
                    Parser.Default.ParseArguments<RoomOption, PeopleOption>(args)
                        .WithParsed<OptionBase>(opt => opt.Execute(app))
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
        
        void ListenClientEvents (Client client)
        {
            client.NewMessage += (sender, e) => Console.WriteLine(e.ToReadString());
            client.MakeFriendHandler = MakeFriend;
        }

        private async Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
        {
            Console.WriteLine($"User {request.SenderId} wants to become your friend.");
            Console.WriteLine($"Greeting: {request.Greeting}");
            while (true)
            {
                Console.Write($"Accept? y/[n]: ");
                var input = Console.ReadLine();
                if(input == "y")
                    return new MakeFriendResponse
                    {
                        Status = MakeFriendResponse.Types.Status.Accept
                    };
                if(input == "n" || string.IsNullOrWhiteSpace(input))
                    return new MakeFriendResponse
                    {
                        Status = MakeFriendResponse.Types.Status.Refuse
                    };
            }
        }

        void ShowMessages (Client client)
        {
            Console.WriteLine("Unread messages:");
            var request = new GetMessagesRequest
            {
                AfterTimeUnixMs = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };
            var messages = client.GetMessages(request).Result;
            foreach(var message in messages)
                Console.WriteLine($"{message.SenderId}: {message.Content.Text}");
        }
    }
}