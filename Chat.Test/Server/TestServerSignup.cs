using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server;
using Xunit;

namespace Chat.Test
{
    public class TestServerSignup: TestServerBase
    {
        [Fact]
		public async Task Signup()
		{
			var t0 = DateTimeOffset.Now;
			var response = await server.SignupAsync(new SignupRequest
			{
				Username = "user1",
				Password = "password"
            });
			var t1 = DateTimeOffset.Now;
			Assert.Equal(new SignupResponse
			{
				Status = SignupResponse.Types.Status.Success,
				UserId = 1
			}, response);

            var user = await server.FindUserAsync(1);
            Assert.NotNull(user);
			Assert.Equal("user1", user.Username);
			Assert.Equal("password", user.Password);
			Assert.Equal(1, user.Id);
			Assert.True(user.CreateTime > t0 && user.CreateTime < t1);
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
			Assert.Equal(SignupResponse.Types.Status.PasswordFormatWrong, response.Status);
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
			Assert.Equal(SignupResponse.Types.Status.UsernameFormatWrong, response.Status);
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
			Assert.Equal(SignupResponse.Types.Status.UsernameExist, response.Status);
		}
	}
}
