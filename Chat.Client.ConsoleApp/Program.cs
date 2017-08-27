﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CommandLine;

namespace Chat.Client.ConsoleApp
{
    using Connection.Grpc;
    using Options;

    class Program
    {
        ConsoleOption copt;
        Client client;

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
                .ConfigureLogger(obj => obj.AddConsole())
                .UseGrpc(copt.ServerAddress, copt.Host, copt.Port)
                .SetUser(opt.UserId, opt.Password);
            client = builder.Build();
            try 
            {
                client.Login().Wait();
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
					var args = Console.ReadLine().Split(' ');
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
