using HikeOrganiser.Data.Entities;
using Events = HikeOrganiser.Core.Behaviours.Events;

namespace HikeOrganiser.Core.AutoMapper;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, Events.GetAll.EventModel>()
            .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.Attendees.Select(a => a.User != null ? a.User.DisplayName : "").ToList()));

        CreateMap<Event, Events.Get.EventModel>().ReverseMap();
    }
}