using System;
using System.Diagnostics;
using Chat.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.ConsoleApp
{
	using Client;
    using Core.Interfaces;
	using Connection.Local;

    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

			var container = new ServiceCollection();
            container.AddSingleton<ILoggerFactory>(loggerFactory);
			var provider = container.BuildServiceProvider();
			// Make Server
			var server = new Server(provider);
			var loginService = new LocalLoginService(server);
            container.AddSingleton<ILoginService>(loginService);
            provider = container.BuildServiceProvider();
	        // Signup
	        var rsp1 = loginService.Signup(new SignupRequest{Username = "user1", Password = "123456"});
	        var rsp2 = loginService.Signup(new SignupRequest{Username = "user2", Password = "123456"});
	        // Login
			var client1 = new Client(rsp1.UserId, provider);
			LocalClientService.Register(client1);
			var client2 = new Client(rsp2.UserId, provider);
			LocalClientService.Register(client2);

			client1.Login("123456");
			client2.Login("123456");

			client2.SendTextMessage("Hello, 1.");

            Console.WriteLine("End");
        }
    }
}
