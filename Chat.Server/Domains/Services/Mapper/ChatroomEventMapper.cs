using System;
using AutoMapper;
using Chat.Core.Models;
using Chat.Server.Domains.Events.Chatroom;

namespace Chat.Server.Domains.Services
{
    static class ChatroomEventMapper
    {
        static IMapper Mapper { get; }

        public static ChatMessage Map(ChatroomEvent e)
        {
            return Mapper.Map<ChatroomEvent, ChatMessage>(e);
        }

        static ChatroomEventMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DateTimeOffset, long>()
                    .ConvertUsing(t => t.ToUnixTimeMilliseconds());
                
                cfg.RecognizeAlias("PeopleId", "UserId");
                
                cfg.CreateMap<ChatroomEvent, ChatMessage>()
                    .ForMember(cm => cm.TimeUnixMs, opt => opt.MapFrom(e => e.Time))
                    .ForMember(cm => cm.Content, opt => opt.MapFrom(e => e));

                cfg.CreateMap<ChatroomEvent, Content>();
                
                cfg.CreateMap<NewMessageEvent, ChatMessage>()
                    .IncludeBase<ChatroomEvent, ChatMessage>()
                    .ConvertUsing(e => e.Message);
                
                cfg.CreateMap<NewChatroomEvent, Content>()
                    .IncludeBase<ChatroomEvent, Content>()
                    .ForMember(c => c.Created, opt => opt.UseValue(new Content.Types.Created()));
                
                cfg.CreateMap<UserEnteredChatroomEvent, Content>()
                    .IncludeBase<ChatroomEvent, Content>()
                    .ForMember(c => c.PeopleEnter, opt => opt.MapFrom(e => e));
                cfg.CreateMap<UserEnteredChatroomEvent, Content.Types.PeopleEnter>();
                
                cfg.CreateMap<UserLeftChatroomEvent, Content>()
                    .IncludeBase<ChatroomEvent, Content>()
                    .ForMember(c => c.PeopleLeave, opt => opt.MapFrom(e => e));   
                cfg.CreateMap<UserLeftChatroomEvent, Content.Types.PeopleLeave>();
                
            }).CreateMapper();
        }
    }
}