using System;
using System.Threading.Tasks;
using Chat.Connection.Grpc;
using Chat.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CommandLine;
using Chat.Server.ConsoleApp.Options;
using System.Collections.Generic;

namespace Chat.Server.ConsoleApp
{
    class Program
    {
        ConsoleOption consoleOption;
        Server server;

		void Signup(SignupOption opt)
		{
			var request = new SignupRequest
			{
				Username = opt.Username,
				Password = opt.Password
			};
            var response = server.Signup(request);
		}

        void Delete (DeleteOption opt)
        {
            // TODO Server delete user
            throw new NotImplementedException();
        }

        void ShowUserInfo(UserInfoOption opt)
        {
            var user = server.GetUser(opt.UserId);
            if(user == null)
            {
                Console.Error.WriteLine($"User {opt.UserId} does not exist.");
                return;
            }
            Console.WriteLine(user);
        }

		void ParseFailed(IEnumerable<Error> obj)
		{
			Console.Error.WriteLine("Error:");
			foreach (var e in obj)
				Console.Error.WriteLine(e);
		}

		void Main(ConsoleOption opt)
		{
            consoleOption = opt;
            var builder = new ServerBuilder()
                .ConfigureLogger(cfg => cfg
                                 .AddFilter((category, level) => 
                                            !category.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.CurrentCulture) 
                                            || level >= LogLevel.Warning)
                                 .AddConsole())
                .UseGrpc(opt.Host, opt.Port);
            if (opt.Database == ConsoleOption.DbType.InMemory)
                builder.UseInMemory();
            else if(opt.Database == ConsoleOption.DbType.SQLite)
                builder.UseSQLite(opt.SQLiteString);
            server = builder.Build();

			while (true)
			{
				Console.Write("> ");
                try
                {
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd))
                        continue;
					var args = cmd.Split(' ');
					Parser.Default
                          .ParseArguments<SignupOption, DeleteOption, UserInfoOption>(args)
						  .WithParsed<SignupOption>(Signup)
                          .WithParsed<DeleteOption>(Delete)
                    .WithParsed<UserInfoOption>(ShowUserInfo)
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
