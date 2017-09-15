using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructure;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Entities
{
    public class User: DomainBase
    {
        public string Username { get; private set; }
        public DateTimeOffset CreateTime { get; private set; } = DateTimeOffset.Now;
        internal ICollection<UserChatroom> UserChatrooms { get; private set; }
	    internal ICollection<UserRelationship> UserRelationships { get; private set; }

        [NotMapped]
        public IEnumerable<long> ChatroomIds => UserChatrooms.Select(uc => uc.ChatroomId);

	    [NotMapped]
	    public IEnumerable<long> FriendIds =>
		    UserRelationships.Where(r => r.IsFriend).Select(r => r.ToUserId);

	    private User()
	    {
		    
	    }

	    internal User(long id, string username)
	    {
		    Id = id;
		    Username = username;
	    }

	    public bool IsFriend(User user)
	    {
		    return UserRelationships.FirstOrDefault(r => r.ToUserId == user.Id)?.IsFriend ?? false;
	    }
	    
	    public bool IsFriend(long userId)
	    {
		    return UserRelationships.FirstOrDefault(r => r.ToUserId == userId)?.IsFriend ?? false;
	    }

        public override string ToString()
        {
            return string.Format("[User: Id={0}, Username={1}, Friends={2}]", Id, Username,
	            FriendIds.ToJsonString());
        }
	    
	    internal UserRelationship GetOrAddRelationshipWith (User target)
	    {
		    var relationship = UserRelationships.FirstOrDefault(r => r.ToUserId == target.Id);
		    if (relationship == null)
		    {
			    relationship = new UserRelationship(this, target);
			    UserRelationships.Add(relationship);
			    Logger?.LogInformation($"New relationship with user {target.Id}");
		    }
		    return relationship;
	    }
	    
	    internal async Task<MakeFriendResponse> HandleMakeFriend(MakeFriendRequest request, User target)
	    {
		    if(target.Id == Id)
			    return new MakeFriendResponse
			    {
				    Status = MakeFriendResponse.Types.Status.WithSelf
			    };
		    if(IsFriend(target))
			    return new MakeFriendResponse
			    {
				    Status = MakeFriendResponse.Types.Status.AlreadyFriend
			    };
			var timeout = TimeSpan.FromSeconds(5);
			var task = target.AskClientToMakeFriend(request);
		    await Task.WhenAny(task, Task.Delay(timeout));
			if(!task.IsCompleted)
				return new MakeFriendResponse
				{
					Status = MakeFriendResponse.Types.Status.ResponseTimeout
				};
		    var response = task.Result;
		    if (response.Status == MakeFriendResponse.Types.Status.Accept)
			    this.MakeFriendsWith(target);
		    return response;
	    }

	    async Task<MakeFriendResponse> AskClientToMakeFriend(MakeFriendRequest request)
	    {
		    var client = _provider.GetRequiredService<UserClientService>()[Id];
		    if (client == null)
			    return new MakeFriendResponse
			    {
				    Status = MakeFriendResponse.Types.Status.UserNotOnline
			    };
		    return await client.MakeFriend(request);
	    }

	    internal void MakeFriendsWith(User user)
	    {
		    this.GetOrAddRelationshipWith(user).SetFriend();
		    user.GetOrAddRelationshipWith(this).SetFriend();
		    _provider.GetRequiredService<IEventBus>().Publish(
			    new BecameFriendsEvent{UserId = Id, User2Id = user.Id});
	    }
    }
}
