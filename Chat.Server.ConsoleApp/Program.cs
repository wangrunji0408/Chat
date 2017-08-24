using System;
using System.Diagnostics;
using Chat.Connection.Grpc;
using Chat.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.ConsoleApp
{
	using Client;
    using Core.Interfaces;
	using Connection.Local;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            TestGrpc().Wait();
        }
	    
	    private static async Task TestGrpc()
	    {
            var server = new ServerBuilder()
				.ConfigureLogger(factory => factory.AddConsole(minLevel: LogLevel.Debug))
                .UseGrpc(8080)
                .Build();
            
		    // Signup
		    server.Signup(new SignupRequest {Username = "user1", Password = "123456"});
		    server.Signup(new SignupRequest {Username = "user2", Password = "123456"});

			// Make Connect
			var clientBuilder = new ClientBuilder()
				.ConfigureLogger(factory => factory.AddConsole(minLevel: LogLevel.Debug))
                .UseGrpc("localhost:8080");
            
            var client1 = clientBuilder.SetUser(1, "123456").Build();
			var client2 = clientBuilder.SetUser(2, "123456").Build();

			await client1.Login();
			await client2.Login();

		    await client2.SendTextMessage("Hello, 1.");

		    Console.WriteLine("End");
	    }

	    private static async Task TestLocal()
	    {

			var server = new ServerBuilder()
				.ConfigureLogger(factory => factory.AddConsole(minLevel: LogLevel.Debug))
                .UseLocal()
				.Build();

			// Signup
			server.Signup(new SignupRequest { Username = "user1", Password = "123456" });
			server.Signup(new SignupRequest { Username = "user2", Password = "123456" });

			// Make Connect
			var clientBuilder = new ClientBuilder()
				.ConfigureLogger(factory => factory.AddConsole(minLevel: LogLevel.Debug))
                .UseLocal(server);

			var client1 = clientBuilder.SetUser(1, "123456").Build();
			var client2 = clientBuilder.SetUser(2, "123456").Build();

			await client1.Login();
			await client2.Login();
		    await client2.SendTextMessage("Hello, 1.");

		    Console.WriteLine("End");
	    }
    }
}
