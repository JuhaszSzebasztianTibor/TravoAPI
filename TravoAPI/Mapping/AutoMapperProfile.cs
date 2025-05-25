using AutoMapper;
using TravoAPI.Dtos.Budget;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Dtos.Packing;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Models.Enums;

namespace TravoAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Trip <=> TripDto
            CreateMap<Trip, TripDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore());

            // Destination <=>DestinationDto
            CreateMap<Destination, DestinationDto>().ReverseMap();

            // DayPla n<=> DayPlanDto
            CreateMap<DayPlan, DayPlanDto>()
                           .ForMember(dto => dto.Location, opt => opt.MapFrom(dp => dp.Location))
                           .ForMember(dto => dto.Places, opt => opt.MapFrom(dp => dp.Places))
                           .ReverseMap()
                           .ForMember(dp => dp.Places, opt => opt.Ignore());

            // Place <=> PlaceDto
            CreateMap<Place, PlaceDto>()
                // expose TripId on the DTO
                .ForMember(dest => dest.TripId,
                           opt => opt.MapFrom(src => src.DayPlan.TripId))
                .ReverseMap()
                // <<< Prevent AutoMapper from creating a new DayPlan entity >>>
                .ForMember(dest => dest.DayPlan, opt => opt.Ignore());

            // Note <=> NoteDto
            CreateMap<Note, NoteDto>().ReverseMap();


            CreateMap<PackingList, PackingListDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TripId, opt => opt.Ignore());

            // PackingItem <=> PackingItemDto
            CreateMap<PackingItem, PackingItemDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PackingList, opt => opt.Ignore());

            // BudgetCreateDto → Budget 
            CreateMap<BudgetCreateDto, Budget>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.TripId, o => o.Ignore())
                .ForMember(d => d.Status,
                           o => o.MapFrom(src => Enum.Parse<BudgetStatus>(src.Status)));

            // Budget → BudgetDto (reply to client)
            CreateMap<Budget, BudgetDto>()
                .ForMember(d => d.Status,
                           o => o.MapFrom(src => src.Status.ToString()));

        }
    }
}
