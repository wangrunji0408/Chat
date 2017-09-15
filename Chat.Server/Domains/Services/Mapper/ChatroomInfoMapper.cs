using System.Linq;
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
                    .ForMember(d => d.Peoples, opt => opt.MapFrom(s => s.UserChatrooms))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                    .AfterMap((s, t) => t.Peoples.Add(
                        s.UserChatrooms.Select(uc => 
                            MemberMapper.Map<UserChatroom, ChatroomInfo.Types.PeopleInRoom>(uc))));

                cfg.CreateMap<UserChatroom, ChatroomInfo.Types.PeopleInRoom>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId))
                    .ForMember(d => d.Name, opt =>
                    {
                        opt.NullSubstitute("");
                        opt.MapFrom(s => s.NameInChatroom);
                    });
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