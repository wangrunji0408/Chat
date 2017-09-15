using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chat.Client.ConsoleApp.Options;
using Chat.Client.ConsoleApp.Options.Chatroom;
using Chat.Connection.Grpc;
using Microsoft.Extensions.Logging;
using CommandLine;
using NLog.Extensions.Logging;

namespace Chat.Client.ConsoleApp
{

    class Program
    {
		const string NlogConfigFile = "nlog.config";
        internal ILoggerFactory LoggerFactory;
        internal ILogger Logger;
        internal ILogger Cmdlogger;

        internal ConsoleOption copt;
        internal ClientBuilder Builder;
        internal Client Client;

        internal void Setup()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddNLog().ConfigureNLog(NlogConfigFile);
            Logger = LoggerFactory.CreateLogger("Chat.Client.Console");
            Cmdlogger = LoggerFactory.CreateLogger("Chat.Client.Console.Commands");
            GrpcConnectionExtension.SetLogger(LoggerFactory);
            Builder = new ClientBuilder()
                .UseLoggerFactory(LoggerFactory)
                .UseGrpc(copt.ServerAddress, copt.Host, copt.Port);
        }
        
        internal void ParseFailed(IEnumerable<Error> obj)
        {
            Console.Error.WriteLine("Error:");
            foreach (var e in obj)
                Console.Error.WriteLine(e);
        }

        void ReadCommands()
        {
            var suboptions = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => t.IsSubclassOf(typeof(RootOptionBase)))
                .ToArray();
            
            while(true)
            {
                Console.Write("> ");
                try
                {
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd))
                        continue;
                    Cmdlogger.LogTrace(cmd);
                    var args = cmd.Split(' ');
                    Parser.Default.ParseArguments(args, suboptions)
                        .WithParsed<RootOptionBase>(opt => opt.Execute(this))
                        .WithNotParsed(ParseFailed);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Client throws an exception. Check 'console-exception.log' for details.");
                    Console.Error.WriteLine(e.Message);
                    Logger.LogError(e, "Client throws an exception.");
                }
            }
        }
        
        static void Main(string[] args)
        {
            var app = new Program();
            Parser.Default
                  .ParseArguments<ConsoleOption>(args)
                  .WithParsed(opt => opt.Execute(app));
            app.Setup();
            app.ReadCommands();
        }
    }
}
