using System;
using System.Collections.Generic;
using Chat.Connection.Grpc;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options
{
    class ConsoleOption: OptionBase
    {
        [Option('s', "server_address", Required = true)]
        public string ServerAddress { get; set; }

        [Option('h', "host", Default = "localhost", HelpText = "Grpc service host.")]
        public string Host { get; set; }

        [Option('p', "port", HelpText = "Client service port.")]
        public int Port { get; set; }

        [Usage(ApplicationAlias = "dotnet run")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Start the client, with a server in localhost",
                    new ConsoleOption { ServerAddress = "localhost:8080" });
                yield return new Example("Start the client, with a server in LAN",
                    new ConsoleOption { ServerAddress = "192.168.1.10:8080", Host = "192.168.1.2" });
            }
        }

        internal override void Execute(Program app)
        {
            app.copt = this;
        }
    }
}