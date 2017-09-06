using System;
using System.Threading;
using System.Threading.Tasks;
using Chat.Connection.Grpc;
using Chat.Core.Models;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options
{
    [Verb("login")]
    class LoginOption: OptionBase
    {
        [Value(0, MetaName = "UserID")]
        public long UserId { get; set; }
        [Value(1, MetaName = "Password")]
        public string Password { get; set; }

        internal override void Execute(Program app)
        {
            if(app.Client != null)
            {
                Console.Error.WriteLine("Already login.");
                return;
            }
            var builder = new ClientBuilder()
                .UseLoggerFactory(app.LoggerFactory)
                .UseGrpc(app.copt.ServerAddress, app.copt.Host, app.copt.Port)
                .SetUser(UserId, Password);
            app.Client = builder.Build();
            try 
            {
                app.Client.Login().Wait();
                ListenClientEvents(app.Client);
                ShowMessages(app.Client);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine($"Failed: {e.Message}");
                app.Client = null;
            }
        }
        
        void ListenClientEvents (Client client)
        {
            client.NewMessage += (sender, e) => Console.WriteLine(
                $"[Room {e.ChatroomId} User {e.SenderId}] {e.Content.Text}");
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
                AfterTimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            var messages = client.GetMessages(request).Result;
            foreach(var message in messages)
                Console.WriteLine($"{message.SenderId}: {message.Content.Text}");
        }
    }
}