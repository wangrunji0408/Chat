using System;
using AutoMapper;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;

namespace Chat.Server.Infrastructure.AutoMapper
{
    static class PeopleInfoMapper
    {
        public static IMapper SelfMapper => SelfMapperLazy.Value;
        public static IMapper FriendMapper => FriendMapperLazy.Value;
        public static IMapper StrangerMapper => StrangerMapperLazy.Value;

        private class PeopleInfoProfile : Profile
        {
            public PeopleInfoProfile()
            {
                CreateMap<DateTimeOffset, long>()
                    .ConvertUsing(t => t.ToUnixTimeMilliseconds());
            }
        }
        
        private static readonly Lazy<IMapper> SelfMapperLazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PeopleInfoProfile>();
                
                cfg.CreateMap<User, PeopleInfo>()
                    .ForMember(i => i.Self, opt => opt.MapFrom(u => u));
                cfg.CreateMap<User, PeopleInfo.Types.SelfInfo>()
                    .ForMember(
                        i => i.SignupTimeUnixMs,
                        opt => opt.MapFrom(u => u.CreateTime));
            });
            return config.CreateMapper();
        });
        
        private static readonly Lazy<IMapper> FriendMapperLazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PeopleInfoProfile>();
                cfg.CreateMap<User, PeopleInfo>()
                    .ForMember(i => i.Friend, opt => opt.MapFrom(u => u));
                cfg.CreateMap<User, PeopleInfo.Types.FriendInfo>();
            });
            return config.CreateMapper();
        });
        
        private static readonly Lazy<IMapper> StrangerMapperLazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PeopleInfoProfile>();
                cfg.CreateMap<User, PeopleInfo>()
                    .ForMember(i => i.Stranger, opt => opt.MapFrom(u => u));
                cfg.CreateMap<User, PeopleInfo.Types.StrangerInfo>();
            });
            return config.CreateMapper();
        });
    }
}