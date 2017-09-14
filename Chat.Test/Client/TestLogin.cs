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
            await clientBuilder.SignupAsync("user1", "password123");
        }
        
        [Fact]
        public async Task Login()
        {
            await clientBuilder.SignupAsync("user1", "password123");
            client1 = await clientBuilder.LoginAsync("user1", "password123");
        }

        [Fact]
        public async Task Login_WrongPassword()
        {
            await clientBuilder.SignupAsync("user1", "password123");
            await Assert.ThrowsAsync<Exception>(async () => 
                await clientBuilder.LoginAsync("user1", "password"));
        }
    }
}