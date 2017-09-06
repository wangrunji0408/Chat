using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public abstract class TestClientLogin<TSetup>: TestClientBase<TSetup>
        where TSetup: SetupBase, new()
    {
        [Fact]
        public async Task Login()
        {
            await loginService.SignupAsync(new SignupRequest{Username = "user1", Password = "123456"});
            var client = clientBuilder.SetUser(1, "123456").Build();
            await client.Login();
        }

        [Fact]
        public async Task Login_WrongPassword()
        {
            await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
            var client = clientBuilder.SetUser(1, "654321").Build();
            await Assert.ThrowsAsync<Exception>(client.Login);
        }
    }
    
    public class TestClientLogin_Local: TestClientLogin<LocalSetup> {}
    public class TestClientLogin_GrpcLocal: TestClientLogin<GrpcLocalSetup> {}
}