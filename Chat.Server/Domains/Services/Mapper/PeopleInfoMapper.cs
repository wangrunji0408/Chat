using System;
using AutoMapper;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;

namespace Chat.Server.Domains.Services
{
    static class PeopleInfoMapper
    {
        public static readonly IMapper SelfMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PeopleInfoProfile>();
                
            cfg.CreateMap<User, PeopleInfo>()
                .ForMember(i => i.Self, opt => opt.MapFrom(u => u));
            cfg.CreateMap<User, PeopleInfo.Types.SelfInfo>()
                .ForMember(
                    i => i.SignupTimeUnixMs,
                    opt => opt.MapFrom(u => u.CreateTime));
        }).CreateMapper();
        
        public static readonly IMapper FriendMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PeopleInfoProfile>();
            cfg.CreateMap<User, PeopleInfo>()
                .ForMember(i => i.Friend, opt => opt.MapFrom(u => u));
            cfg.CreateMap<User, PeopleInfo.Types.FriendInfo>();
        }).CreateMapper();
        
        public static readonly IMapper StrangerMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PeopleInfoProfile>();
            cfg.CreateMap<User, PeopleInfo>()
                .ForMember(i => i.Stranger, opt => opt.MapFrom(u => u));
            cfg.CreateMap<User, PeopleInfo.Types.StrangerInfo>();
        }).CreateMapper();

        private class PeopleInfoProfile : Profile
        {
            public PeopleInfoProfile()
            {
                CreateMap<DateTimeOffset, long>()
                    .ConvertUsing(t => t.ToUnixTimeMilliseconds());
            }
        }
    }
}