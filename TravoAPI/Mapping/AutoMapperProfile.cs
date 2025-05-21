using AutoMapper;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;

namespace TravoAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Trip, TripDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image))  // Add this line
                .ForMember(d => d.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore());         // ignore Id when mapping from DTO to entity (EF handles Id)\
            CreateMap<Destination, DestinationDto>().ReverseMap();
            CreateMap<DayPlan, DayPlanDto>().ReverseMap();
            CreateMap<Place, PlaceDto>().ReverseMap();
            CreateMap<Note, NoteDto>().ReverseMap();
        }
    }
}
