using System;
namespace Chat.Server.Domains
{
    public class UserChatroom
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }

        public string NameInChatroom { get; set; }

        public UserChatroom()
        {
        }
    }
}
