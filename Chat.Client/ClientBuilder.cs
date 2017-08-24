using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Chat.Core.Interfaces;

namespace Chat.Client
{
    public sealed class ClientBuilder
    {
        long _userId;
        string _username;
        string _password;
        readonly ILoggerFactory _loggerFactory;
        readonly IServiceCollection _serviceCollection;
        ClientConnectionBuilder _connection;

        public ClientBuilder ()
        {
            _serviceCollection = new ServiceCollection();
            _loggerFactory = new LoggerFactory();
            _serviceCollection.AddSingleton(_loggerFactory);
        }

        public Client Build()
        {
            var loginService = _connection.Before();
            _serviceCollection.AddSingleton(loginService);
            var provider = _serviceCollection.BuildServiceProvider();
            var client = new Client(provider)
            {
                UserId = _userId,
                Password = _password,
            };
            _connection.After(client, provider);
            return client;
        }

        public ClientBuilder SetUser (long userId, string password)
        {
            //_username = username;
            _userId = userId;
            _password = password;
            return this;
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
    }

    public abstract class ClientConnectionBuilder
    {
        public abstract ILoginService Before();
        public abstract void After(Client client, IServiceProvider provider);
    }
}
