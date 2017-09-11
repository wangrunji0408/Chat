using AutoMapper;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;

namespace Chat.Server.Domains.Services
{
    static class ChatroomInfoMapper
    {
        public static IMapper MemberMapper { get; }
        public static IMapper NonMemberMapper { get; }

        static ChatroomInfoMapper()
        {
            MemberMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Chatroom, ChatroomInfo>()
                    .ForMember(i => i.PeopleIds, opt => opt.MapFrom(c => c.UserIds))
                    .AfterMap((c, i) => i.PeopleIds.Add(c.UserIds));
            }).CreateMapper();
            
            NonMemberMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Chatroom, ChatroomInfo>()
                    .ForMember(i => i.Id, opt => opt.MapFrom(c => c.Id))
                    .ForAllOtherMembers(opt => opt.Ignore());
            }).CreateMapper();
        }
    }
}