using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    public sealed class ServerBuilder
    {
		readonly ILoggerFactory _loggerFactory;
		readonly IServiceCollection _serviceCollection;
        ServerConnectionBuilder _connection;

        public ServerBuilder()
        {
			_serviceCollection = new ServiceCollection();
			_loggerFactory = new LoggerFactory();
			_serviceCollection.AddSingleton(_loggerFactory);
        }

		public Server Build()
		{
			var provider = _serviceCollection.BuildServiceProvider();
            var server = new Server(provider);
			_connection?.After(server, provider);
			return server;
		}

		public ServerBuilder SetConnection(ServerConnectionBuilder connection)
		{
			_connection = connection;
			return this;
		}

		public ServerBuilder ConfigureLogger(Action<ILoggerFactory> config)
		{
			config(_loggerFactory);
			return this;
		}
    }

    public abstract class ServerConnectionBuilder
    {
        public abstract void After(Server server, IServiceProvider provider);
    }
}
