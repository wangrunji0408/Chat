using System;
namespace Chat.Server.Domains
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format("[User: Id={0}, Username={1}, Password={2}]", Id, Username, Password);
        }
    }
}
