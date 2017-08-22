using System;
using System.Diagnostics;
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

			var server = new Server(provider);
			var loginService = new LocalLoginService(server);
            container.AddSingleton<ILoginService>(loginService);
            provider = container.BuildServiceProvider();

			var client1 = new Client(1, provider);
			LocalClientService.Register(client1);

			var client2 = new Client(2, provider);
			LocalClientService.Register(client2);

			client1.Login();
			client2.Login();

			client2.SendMessage("Hello, 1.");

            Console.WriteLine("End");
        }
    }
}
