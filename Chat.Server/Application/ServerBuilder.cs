using System;
using Chat.Server.Domains.Repositories;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    public sealed class ServerBuilder
    {
		ILoggerFactory _loggerFactory;
		readonly IServiceCollection _container;
        ServerConnectionBuilder _connection;

        public ServerBuilder()
        {
			_container = new ServiceCollection();
            _container.AddSingleton<ILoggerFactory>(_loggerFactory = new LoggerFactory());
	        _container.AddSingleton<IEventBus, EventBus>();
	        _container.AddSingleton<IUserRepository, UserRepository>();
	        _container.AddSingleton<IChatroomRepository, ChatroomRepository>();
	        _container.AddSingleton<IMessageRepository, MessageRepository>();
	        _container.AddSingleton<UserService>();
	        _container.AddSingleton<ChatroomService>();
	        _container.AddSingleton<MessageService>();
        }

		public Server Build()
		{
			var provider = _container.BuildServiceProvider();
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
            _container.AddSingleton(_loggerFactory = factory);
			return this;
		}

        public ServerBuilder ConfigureDbContext (Action<DbContextOptionsBuilder> optionsAction)
        {
            _container.AddDbContext<ServerDbContext>(optionsAction);
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
