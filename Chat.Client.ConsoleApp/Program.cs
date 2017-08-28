using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CommandLine;
using NLog.Extensions.Logging;

namespace Chat.Client.ConsoleApp
{
    using Connection.Grpc;
    using Options;

    class Program
    {
        ConsoleOption copt;
        Client client;

        void ListenClientEvents ()
        {
            client.NewMessage += (sender, e) => Console.WriteLine($"{e.SenderId}: {e.Content.Text}");
        }

        void Signup (SignupOption opt)
        {
            
        }

        void Login (LoginOption opt)
        {
            if(client != null)
            {
                Console.Error.WriteLine("Already login.");
                return;
            }
            var builder = new ClientBuilder()
                .ConfigureLogger(obj => obj.AddNLog().ConfigureNLog("nlog.config"))
                .UseGrpc(copt.ServerAddress, copt.Host, copt.Port)
                .SetUser(opt.UserId, opt.Password);
            client = builder.Build();
            try 
            {
                client.Login().Wait();
                ListenClientEvents();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine($"Failed: {e.Message}");
                client = null;
            }
        }

        void Send (SendOption opt)
        {
            client.SendTextMessage(opt.Text).Wait();
        }

		void ParseFailed(IEnumerable<Error> obj)
		{
            Console.Error.WriteLine("Error:");
            foreach (var e in obj)
                Console.Error.WriteLine(e);
		}

        void Main (ConsoleOption opt)
        {
            copt = opt;
            while(true)
            {
                Console.Write("> ");
                try
                {
					var cmd = Console.ReadLine();
					if (string.IsNullOrWhiteSpace(cmd))
						continue;
					var args = cmd.Split(' ');
					Parser.Default
						  .ParseArguments<SignupOption, LoginOption, SendOption>(args)
						  .WithParsed<SignupOption>(Signup)
						  .WithParsed<LoginOption>(Login)
						  .WithParsed<SendOption>(Send)
						  .WithNotParsed(ParseFailed);
                }
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
				}
            }
        }

        static void Main(string[] args)
        {
            var app = new Program();
            Parser.Default
                  .ParseArguments<ConsoleOption>(args)
                  .WithParsed(app.Main);
        }
    }
}
