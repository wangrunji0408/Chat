﻿using System;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    public sealed class ServerBuilder
    {
		ILoggerFactory _loggerFactory;
		readonly IServiceCollection _serviceCollection;
        ServerConnectionBuilder _connection;

        public ServerBuilder()
        {
			_serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton<ILoggerFactory>(_loggerFactory = new LoggerFactory());
	        _serviceCollection.AddSingleton<IEventBus, EventBus>();
        }

		public Server Build()
		{
			var provider = _serviceCollection.BuildServiceProvider();
            if (provider.GetService<ServerDbContext>() == null)
                throw new InvalidOperationException("ConfigureDbContext must be called before build.");
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

		public ServerBuilder UseLoggerFactory(ILoggerFactory factory)
		{
            _serviceCollection.AddSingleton(_loggerFactory = factory);
			return this;
		}

        public ServerBuilder ConfigureDbContext (Action<DbContextOptionsBuilder> optionsAction)
        {
            _serviceCollection.AddDbContext<ServerDbContext>(optionsAction);
            return this;
        }

		public ServerBuilder UseSQLite(string connectionString)
		{
            return ConfigureDbContext(builder => builder.UseSqlite(connectionString));
		}

		public ServerBuilder UseInMemory(string name = "database")
		{
            return ConfigureDbContext(builder => builder.UseInMemoryDatabase(name));
		}
    }

    public abstract class ServerConnectionBuilder
    {
        public abstract void After(Server server, IServiceProvider provider);
    }
}
