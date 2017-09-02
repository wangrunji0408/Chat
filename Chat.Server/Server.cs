using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
    using Core.Interfaces;
    using Core.Models;
    using Domains;

    public class Server
    {
        readonly ILogger _logger;
        readonly ServerDbContext _context;

        readonly UserService _userService;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server");
            _context = serviceProvider.GetRequiredService<ServerDbContext>();
            _context.ServiceProvider = serviceProvider;
            _context.Database.EnsureCreated();
            try {_context.Database.Migrate();}
            catch(Exception e)
            {
                _logger?.LogError(e, "An error occurred when database migrate. It's normal when using InMemory database.");
            }

            _userService = new UserService(serviceProvider);
            EnsureGlobalChatroomCreated();
        }

        void EnsureGlobalChatroomCreated ()
        {
            if (_context.Find<Chatroom>(1L) != null)
                return;
            var chatroom = new Chatroom();
            _context.Add(chatroom);
            _context.SaveChanges();
        }

        public LoginResponse Login (LoginRequest request)
        {
            return _userService.Login(request);
        }

        public SignupResponse Signup (SignupRequest request)
        {
            return _userService.Signup(request);
        }

        public User GetUserNullable (long userId)
        {
            return _context.Find<User>(userId);
        }

        User GetUser (long userId)
        {
            return GetUserNullable(userId)
                ?? throw new InvalidOperationException($"User {userId} does not exist.");
        }

        public Chatroom GetChatroomNullable (long chatroomId)
        {
            return _context.Find<Chatroom>(chatroomId);
        }
		public List<Chatroom> GetChatrooms()
		{
            return _context.Chatrooms.ToList();
		}

        public void ClearDatabase ()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Database.Migrate();
        }

        public void SetUserClient (long userId, IClientService client)
        {
            GetUser(userId).ClientService = client;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            var chatroom = await _context.Chatrooms.FindAsync(message.ChatroomId)
                                   ?? throw new ArgumentException($"Chatroom {message.ChatroomId} does not exist.");
            await chatroom.NewMessage(message);
		}

        public Task<List<ChatMessage>> GetMessages(GetMessagesRequest request)
		{
            var user = GetUser(request.UserId);
            var time = DateTimeOffset.FromUnixTimeSeconds(request.AfterTimeUnix);
            return user.GetMessagesAfter(time);
		}

        public Task<List<ChatMessage>> GetRecentMessages (int count)
        {
            return _context.Messages.OrderByDescending(m => m.Time).Take(count).ToListAsync();
        }
    }
}
