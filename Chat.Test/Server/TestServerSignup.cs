using System;
using Chat.Core.Models;
using Chat.Server;
using Xunit;

namespace Chat.Test
{
    public class TestServerSignup: TestServerBase
    {
        [Fact]
		public void Signup()
		{
			var t0 = DateTimeOffset.Now;
			var response = server.Signup(new SignupRequest
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

            var user = server.FindUserAsync(1).Result;
            Assert.NotNull(user);
			Assert.Equal("user1", user.Username);
			Assert.Equal("password", user.Password);
			Assert.Equal(1, user.Id);
			Assert.True(user.CreateTime > t0 && user.CreateTime < t1);
		}

        [Theory]
		[InlineData("")]
		public void Signup_InvalidPassword(string password)
		{
			var response = server.Signup(new SignupRequest
			{
				Username = "user1",
				Password = password
			});
			Assert.Equal(SignupResponse.Types.Status.PasswordFormatWrong, response.Status);
		}

		[Theory]
		[InlineData("")]
		public void Signup_InvalidUsername(string username)
		{
			var response = server.Signup(new SignupRequest
			{
				Username = username,
				Password = "password"
			});
			Assert.Equal(SignupResponse.Types.Status.UsernameFormatWrong, response.Status);
		}

		[Fact]
		public void Signup_UsernameExist()
		{
			var request = new SignupRequest
			{
				Username = "user1",
				Password = "password"
			};
			server.Signup(request);
			var response = server.Signup(request);
			Assert.Equal(SignupResponse.Types.Status.UsernameExist, response.Status);
		}
	}
}
