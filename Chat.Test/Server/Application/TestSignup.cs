using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Server
{
    public class TestSignup : TestServerBase
    {
        protected override void Signup2User()
        {
            // Disable default signup
        }

        [Theory]
        [InlineData("")]
        public async Task Signup_InvalidPassword(string password)
        {
            var response = await server.SignupAsync(new SignupRequest
            {
                Username = "user1",
                Password = password
            });
            Assert.False(response.Success);
        }

        [Theory]
        [InlineData("")]
        public async Task Signup_InvalidUsername(string username)
        {
            var response = await server.SignupAsync(new SignupRequest
            {
                Username = username,
                Password = "password"
            });
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Signup()
        {
            var t0 = DateTimeOffset.Now;
            var response = await server.SignupAsync(new SignupRequest
            {
                Username = "user1",
                Password = "password123"
            });
            var t1 = DateTimeOffset.Now;
            Assert.Equal(new SignupResponse
            {
                Success = true,
                UserId = 1
            }, response);

            var user = await server.FindUserAsync(1);
            Assert.NotNull(user);
            Assert.Equal("user1", user.Username);
            Assert.Equal(1, user.Id);
            Assert.True(user.CreateTime > t0 && user.CreateTime < t1);
        }

        [Fact]
        public async Task Signup_UsernameExist()
        {
            var request = new SignupRequest
            {
                Username = "user1",
                Password = "password"
            };
            await server.SignupAsync(request);
            var response = await server.SignupAsync(request);
            Assert.False(response.Success);
        }
    }
}