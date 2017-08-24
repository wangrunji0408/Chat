using System;
using System.IO;
using System.Threading.Tasks;
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
    using Connection.Grpc;

    public abstract class TestClient_Base
    {
        protected ClientBuilder clientBuilder;
        protected Server server;

        public TestClient_Base ()
        {
            
        }

	    [Fact]
        public async Task Login()
        {
            server.Signup(new SignupRequest{Username = "user1", Password = "123456"});
            var client = clientBuilder.SetUser(1, "123456").Build();
            await client.Login();
        }

	    [Fact]
        public async Task Login_WrongPassword()
	    {
			server.Signup(new SignupRequest { Username = "user1", Password = "123456" });
			var client = clientBuilder.SetUser(1, "654321").Build();
            await Assert.ThrowsAsync<Exception>(client.Login);
	    }

		[Fact]
		public async Task SendMessage()
		{
			server.Signup(new SignupRequest { Username = "user1", Password = "123456" });
			server.Signup(new SignupRequest { Username = "user2", Password = "123456" });
			var client1 = clientBuilder.SetUser(1, "123456").Build();
			var client2 = clientBuilder.SetUser(2, "123456").Build();
            bool recv1 = false;
            bool recv2 = false;
            string message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
			client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;
			await client1.Login();
			await client2.Login();
            await client2.SendTextMessage(message);

			Assert.True(recv1);
			Assert.True(recv2);
		}
    }

    public class TestClient_Local: TestClient_Base
    {
        public TestClient_Local()
        {
			var serverBuilder = new ServerBuilder().UseLocal();
			server = serverBuilder.Build();
			clientBuilder = new ClientBuilder().UseLocal(server);
        }
    }

	public class TestClient_Grpc : TestClient_Base
	{
		public TestClient_Grpc()
		{
            var serverBuilder = new ServerBuilder().UseGrpc(port:8080);
			
            clientBuilder = new ClientBuilder().UseGrpc("localhost:8080");

            server = serverBuilder.Build();
		}
	}
}
