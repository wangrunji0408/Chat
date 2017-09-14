using System;
using System.Threading.Tasks;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Chat.Test.Server.Infrastructure
{
    public class TestAspNetIdentity
    {
        private UserManager<ChatIdentityUser> _userManager;
        private SignInManager<ChatIdentityUser> _signInManager;
        
        public TestAspNetIdentity()
        {
            var container = new ServiceCollection();
            container.AddDbContext<ServerDbContext>(opt => 
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            container.AddIdentity<ChatIdentityUser, ChatIdentityRole>(opt =>
                {
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ServerDbContext>()
                .AddDefaultTokenProviders();
            var provider = container.BuildServiceProvider();

            _userManager = provider.GetRequiredService<UserManager<ChatIdentityUser>>();
            _signInManager = provider.GetRequiredService<SignInManager<ChatIdentityUser>>();
        }

        const string username = "user1";
        const string password = "password123";
        private ChatIdentityUser iuser;
        
        async Task JustSignup()
        {
            iuser = new ChatIdentityUser(username);
            var result = await _userManager.CreateAsync(iuser, password);
            Assert.True(result.Succeeded);
        }
        
        [Fact]
        public async Task Signup()
        {
            await JustSignup();
            Assert.Equal(username, iuser.UserName);
            Assert.Equal(1, iuser.Id);
            Assert.Equal(iuser, await _userManager.FindByNameAsync("user1"));
        }
        
        [Fact]
        public async Task FindById()
        {
            await JustSignup();
            Assert.Equal(iuser, await _userManager.FindByIdAsync("1"));
        }
        
        [Fact]
        public async Task Login()
        {
            await JustSignup();
            var result = await _signInManager.CheckPasswordSignInAsync(iuser, password, false);
//            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            Assert.True(result.Succeeded);
            
            result = await _signInManager.CheckPasswordSignInAsync(iuser, "null", false);
            Assert.False(result.Succeeded);
            
        }
        
        [Fact]
        public async Task Token()
        {
            await JustSignup();
            var token = await _userManager.GenerateUserTokenAsync(iuser, TokenOptions.DefaultProvider, "login");
            var ok = await _userManager.VerifyUserTokenAsync(iuser, TokenOptions.DefaultProvider, "login", token);
            Assert.True(ok);
        }
    }
}