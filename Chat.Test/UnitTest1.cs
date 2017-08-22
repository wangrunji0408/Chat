using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Test
{
    using Client;
    using Server;
    using Core.Interfaces;
    using Connection.Local;


    public class UnitTest1
    {
        readonly IServiceProvider provider;

        public UnitTest1 ()
        {
			var container = new ServiceCollection();
			provider = container.BuildServiceProvider();

			var server = new Server(provider);
            var loginService = new LocalLoginService(server);
            container.AddSingleton<ILoginService>(loginService);
			provider = container.BuildServiceProvider();

			LocalClientService.Clear();
        }

        public Client NewClient (long userId)
        {
            var client1 = new Client(userId, provider);
			LocalClientService.Register(client1);
			client1.Login();
            return client1;
        }

        [Fact]
        public void Login()
        {
            NewClient(1);
        }

		[Fact]
		public void SendMessage()
		{
			var client1 = NewClient(1);
			var client2 = NewClient(2);
            bool recv1 = false;
            bool recv2 = false;
            string message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Message == message;
			client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Message == message;

            client2.SendMessage(message);

			Assert.True(recv1);
			Assert.True(recv2);
		}
    }
}
