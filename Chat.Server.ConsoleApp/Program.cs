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
		    var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(minLevel: LogLevel.Debug);

		    var container = new ServiceCollection();
		    container.AddSingleton<ILoggerFactory>(loggerFactory);
		    var provider = container.BuildServiceProvider();
		    // Make Server
		    var server = new Server(provider);
		    var grpcServerService = new GrpcServerServiceImpl(server, provider, 8080);
		   
		    // Signup
		    var rsp1 = server.Signup(new SignupRequest {Username = "user1", Password = "123456"});
		    var rsp2 = server.Signup(new SignupRequest {Username = "user2", Password = "123456"});
		    
		    // Make Connect
            var grpcServerServiceClient = new GrpcServerServiceClient("localhost:8080"); // for client1
		    container.AddSingleton<ILoginService>(grpcServerServiceClient);
		    provider = container.BuildServiceProvider();
		    var client1 = new Client(rsp1.UserId, provider);
		    var cs1 = new GrpcClientServiceImpl(client1, grpcServerServiceClient, provider, 8081);

            grpcServerServiceClient = new GrpcServerServiceClient("localhost:8080"); // for client1
		    container.AddSingleton<ILoginService>(grpcServerServiceClient);
		    provider = container.BuildServiceProvider();
		    var client2 = new Client(rsp2.UserId, provider);
		    var cs2 = new GrpcClientServiceImpl(client2, grpcServerServiceClient, provider, 8082);

			await client1.Login("123456");
			await client2.Login("123456");

		    client2.SendTextMessage("Hello, 1.");

		    Console.WriteLine("End");
	    }

	    private static async Task TestLocal()
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
		    var rsp1 = await loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "123456"});
		    var rsp2 = await loginService.SignupAsync(new SignupRequest {Username = "user2", Password = "123456"});
		    // Login
		    var client1 = new Client(rsp1.UserId, provider);
		    LocalClientService.Register(client1);
		    var client2 = new Client(rsp2.UserId, provider);
		    LocalClientService.Register(client2);

		    await client1.Login("123456");
		    await client2.Login("123456");

		    client2.SendTextMessage("Hello, 1.");

		    Console.WriteLine("End");
	    }
    }
}
