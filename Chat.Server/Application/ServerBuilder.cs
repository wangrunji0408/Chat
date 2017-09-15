using System;
using System.Linq;
using System.Reflection;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Chat.Server.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
	        _container.AddIdentity<ChatIdentityUser, ChatIdentityRole>(opt =>
		        {
			        opt.Password.RequiredLength = 8;
			        opt.Password.RequireNonAlphanumeric = false;
			        opt.Password.RequireDigit = false;
			        opt.Password.RequireUppercase = false;
			        // TODO
		        })
		        .AddEntityFrameworkStores<ServerDbContext>()
		        .AddDefaultTokenProviders();
	        
	        
	        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
		        .Where(type => type.Namespace == "Chat.Server.Domains.Services"
		                       && !type.IsAbstract && type.Name.EndsWith("Service"));
	        foreach (var service in serviceTypes)
		        _container.AddSingleton(service);
        }

	    public Server Build()
		{
			var provider = _container.BuildServiceProvider();
			EnsureDatabaseCreated(provider);
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
//	        _container.AddDbContext<IdentityDbContext<ChatIdentityUser, ChatIdentityRole, long>>(optionsAction);
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
	    
	    private void EnsureDatabaseCreated(IServiceProvider provider)
	    {
		    foreach (var context in provider.GetServices<DbContext>())
		    {
			    context.Database.EnsureCreated();
			    try
			    {
				    context.Database.Migrate();
			    }
			    catch (Exception e)
			    {
				    _loggerFactory.CreateLogger<ServerBuilder>()
					    .LogError(e, "An error occurred when database migrate. It's normal when using InMemory database.");
			    }
		    }
	    }
    }

    public abstract class ServerConnectionBuilder
    {
        public abstract void After(Server server, IServiceProvider provider);
    }
}
