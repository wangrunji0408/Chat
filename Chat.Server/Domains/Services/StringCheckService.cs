using System;
using System.Linq;

namespace Chat.Server.Domains.Services
{
    public static class StringCheckService
    {
        const string InvalidCharSet = "!@#$%^&*()";

        public static bool CheckUsername (string username, out string failedReason)
        {
            failedReason = null;
            if (string.IsNullOrWhiteSpace(username))
                failedReason = "Is empty.";
            else if (username.ContainsChar(InvalidCharSet))
                failedReason = "Contains invalid char: " + InvalidCharSet;
            return failedReason == null;
        }

		public static bool CheckPassword(string password, out string failedReason)
		{
            return CheckUsername(password, out failedReason);
		}

        static bool ContainsChar (this string str, string chars)
        {
            return str.Any(chars.Contains);
        }
    }
}
