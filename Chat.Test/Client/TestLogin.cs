using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestLogin<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        protected override async Task SignupAndLoginAsync()
        {
            // Disable default login
        }

        [Fact]
        public async Task Signup()
        {
            var rsp = await loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "password123"});
            Assert.True(rsp.Success, rsp.Detail);
        }
        
        [Fact]
        public async Task Login()
        {
            await loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "password123"});
            client1 = await clientBuilder.Login("user1", "password123");
        }

        [Fact]
        public async Task Login_WrongPassword()
        {
            await loginService.SignupAsync(new SignupRequest {Username = "user1", Password = "password123"});
            await Assert.ThrowsAsync<Exception>(async () => client1 = await clientBuilder.Login("user1", "password"));
        }
    }
}