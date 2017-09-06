using System;

namespace Chat.Server.Domains
{
    /// <summary>
    /// 用户之间的单向关系。如朋友
    /// </summary>
    class UserRelationship
    {
        public long FromUserId { get; private set; }
        public User FromUser { get; private set; }
        public long ToUserId { get; private set; }
//        public User ToUser { get; private set; }
        
        public bool IsFriend { get; private set; }
        public bool IsBlocked { get; private set; }
        public string Note { get; private set; }

        private UserRelationship()
        {
            
        }

        internal UserRelationship(User fromUser, User toUser)
        {
            FromUser = fromUser;
            FromUserId = fromUser.Id;
            ToUserId = toUser.Id;
        }

        internal void SetFriend()
        {
            if(IsFriend == true)
                throw new InvalidOperationException("They are already friends.");
            IsFriend = true;
        }
    }
}