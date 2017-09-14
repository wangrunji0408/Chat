using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Chat.Core.Interfaces;
using Chat.Core.Models;

namespace Chat.Client
{
    public sealed class ClientBuilder
    {
        ILoggerFactory _loggerFactory;
        readonly IServiceCollection _serviceCollection;
        ClientConnectionBuilder _connection;

        public ClientBuilder ()
        {
            _serviceCollection = new ServiceCollection();
            _loggerFactory = new LoggerFactory();
            _serviceCollection.AddSingleton(_loggerFactory);
        }

        public async Task<Client> LoginAsync(string username, string password)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };
            var loginService = _connection.Before();
            var response = await loginService.LoginAsync(request);
            if(!response.Success)
                throw new Exception($"Failed to login. {response.Detail}");
            var provider = _serviceCollection.BuildServiceProvider();
            var client = new Client(provider){UserId = response.UserId};
            _connection.After(client, provider);
            client._serverService = await loginService.GetService(response);
            return client;
        }
        
        public async Task SignupAsync(string username, string password)
        {
            var request = new SignupRequest
            {
                Username = username,
                Password = password
            };
            var loginService = _connection.Before();
            var response = await loginService.SignupAsync(request);
            if(!response.Success)
                throw new Exception($"Failed to signup. {response.Detail}");
        }

        public ClientBuilder SetConnection (ClientConnectionBuilder connection)
        {
            _connection = connection;
            return this;
        }

        public ClientBuilder ConfigureLogger (Action<ILoggerFactory> config)
        {
            config(_loggerFactory);
            return this;
        }

		public ClientBuilder UseLoggerFactory(ILoggerFactory factory)
		{
			_serviceCollection.AddSingleton(_loggerFactory = factory);
			return this;
		}
    }

    public abstract class ClientConnectionBuilder
    {
        public abstract ILoginService Before();
        public abstract void After(Client client, IServiceProvider provider);
    }
}
