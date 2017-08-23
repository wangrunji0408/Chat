using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Test
{
    using Client;
    using Server;
    using Core.Interfaces;
    using Core.Models;
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

	    private Client NewClient (string username, string password)
        {
            var userId = Signup(username, password);

	        var client1 = new Client(userId, provider);
			LocalClientService.Register(client1);
            client1.Login(password).Wait();
            return client1;
        }

	    private long Signup(string username, string password)
	    {
		    var loginService = provider.GetRequiredService<ILoginService>();
		    var request = new SignupRequest {Username = username, Password = password};
            var response = loginService.SignupAsync(request).Result;
		    var userId = response.UserId;
		    return userId;
	    }

	    [Fact]
        public void Login()
        {
            NewClient("user1", "123456");
        }

	    [Fact]
	    public void Login_WrongPassword()
	    {
		    long userId = Signup("user1", "123456");
		    var client = new Client(userId, provider);
		    LocalClientService.Register(client);
		    Assert.ThrowsAsync<Exception>(async () => await client.Login("654321"));
	    }

		[Fact]
		public void SendMessage()
		{
			var client1 = NewClient("user1", "123456");
			var client2 = NewClient("user2", "123456");
            bool recv1 = false;
            bool recv2 = false;
            string message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
			client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;

            client2.SendTextMessage(message);

			Assert.True(recv1);
			Assert.True(recv2);
		}
    }
}
