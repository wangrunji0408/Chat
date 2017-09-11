using System;
using AutoMapper;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;

namespace Chat.Server.Domains.Services
{
    static class GetInfoService
    {
        public static PeopleInfo GetPeopleInfo(User fromUser, User target)
        {
            IMapper mapper;
            if(target.Id == fromUser.Id)
                mapper = PeopleInfoMapper.SelfMapper;
            else if (fromUser.IsFriend(target))
                mapper = PeopleInfoMapper.FriendMapper;
            else
                mapper = PeopleInfoMapper.StrangerMapper;
            return mapper.Map<User, PeopleInfo>(target);
        }
        
        public static ChatroomInfo GetChatroomInfo(User fromUser, Chatroom chatroom)
        {
            IMapper mapper;
            if (chatroom.Contains(fromUser))
                mapper = ChatroomInfoMapper.MemberMapper;
            else
                mapper = ChatroomInfoMapper.NonMemberMapper;
            return mapper.Map<Chatroom, ChatroomInfo>(chatroom);
        }
    }
}