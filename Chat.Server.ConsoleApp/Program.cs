using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CommandLine;
using NLog.Extensions.Logging;

using Chat.Server.ConsoleApp.Options;
using Chat.Connection.Grpc;
using Chat.Core.Models;

namespace Chat.Server.ConsoleApp
{
    class Program
    {
        const string NlogConfigFile = "nlog.config";

        ConsoleOption consoleOption;
        Server server;
        ILogger _logger;
        ILogger _cmdlogger;

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
            var user = server.GetUserNullable(opt.UserId);
            if(user == null)
            {
                Console.Error.WriteLine($"User {opt.UserId} does not exist.");
                return;
            }
            Console.WriteLine(user);
        }

        void ShowMessage (MessageOption opt)
        {
            var messages = server.GetRecentMessages(opt.Count).Result;
            messages.ForEach(Console.WriteLine);
        }

        void ClearDatabase (DatabaseClearOption opt)
        {
            Console.Write("Are you sure to clear database? y/[n]: ");
            var input = Console.ReadLine();
            if(input.ToLower() == "y")
            {
                server.ClearDatabase();
                Console.WriteLine("Database cleared.");
            }
        }

		void ParseFailed(IEnumerable<Error> obj)
		{
			Console.Error.WriteLine("Error:");
			foreach (var e in obj)
				Console.Error.WriteLine(e);
		}

		void Main(ConsoleOption opt)
		{
            var lf = new LoggerFactory();
			lf.AddNLog().ConfigureNLog(NlogConfigFile);
            _logger = lf.CreateLogger("Server.Console");
            _cmdlogger = lf.CreateLogger("Server.Console.Commands");

            consoleOption = opt;
            var builder = new ServerBuilder()
                .ConfigureLogger(cfg => cfg.AddNLog().ConfigureNLog(NlogConfigFile))
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
                    _cmdlogger.LogTrace(cmd);
					var args = cmd.Split(' ');
					Parser.Default
                          .ParseArguments<SignupOption, DeleteOption, UserInfoOption, MessageOption, DatabaseClearOption>(args)
						  .WithParsed<SignupOption>(Signup)
                          .WithParsed<DeleteOption>(Delete)
                            .WithParsed<UserInfoOption>(ShowUserInfo)
                            .WithParsed<MessageOption>(ShowMessage)
                            .WithParsed<DatabaseClearOption>(ClearDatabase)
						  .WithNotParsed(ParseFailed);
                }
				catch (Exception e)
				{
                    Console.Error.WriteLine("Server throws an exception. Check 'console-exception.log' for details.");
                    Console.Error.WriteLine(e.Message);
                    _logger.LogError(e, "Server throws an exception.");
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
