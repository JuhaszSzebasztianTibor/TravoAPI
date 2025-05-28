using AutoMapper;
using TravoAPI.Dtos.Planner;
using TravoAPI.Dtos.Budget;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Dtos.Packing;
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

            // Destination <=> DestinationDto
            CreateMap<Destination, DestinationDto>().ReverseMap();

            // === DayPlan mappings ===

            // Incoming DTO → Entity
            CreateMap<DayPlanDto, DayPlan>()
                .ForMember(dp => dp.Places, opt => opt.Ignore())
                .ForMember(dp => dp.Destination, opt => opt.Ignore())
                .ForMember(dp => dp.TripId, opt => opt.MapFrom(dto => dto.TripId))
                .ForMember(dp => dp.DestinationId, opt => opt.MapFrom(dto => dto.DestinationId));

            // Outgoing Entity → DTO
            CreateMap<DayPlan, DayPlanDto>()
                .ForMember(dto => dto.Places, opt => opt.MapFrom(src => src.Places));

            // === Place mappings ===

            // Outgoing Entity → DTO
            CreateMap<Place, PlaceDto>()
                .ForMember(dest => dest.DayPlanId,
                           opt => opt.MapFrom(src => src.DayPlanId))
                .ForMember(dest => dest.TripId,
                           opt => opt.MapFrom(src => src.DayPlan.TripId));

            // Incoming DTO → Entity
            CreateMap<PlaceDto, Place>()
                .ForMember(p => p.DayPlan, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore());

            // === Note mappings ===

            CreateMap<Note, NoteDto>().ReverseMap();

            // === Packing list mappings ===

            CreateMap<PackingList, PackingListDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TripId, opt => opt.Ignore());

            CreateMap<PackingItem, PackingItemDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PackingList, opt => opt.Ignore());

            // === Budget mappings ===

            CreateMap<BudgetCreateDto, Budget>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.TripId, o => o.Ignore())
                .ForMember(d => d.Status,
                           o => o.MapFrom(src => Enum.Parse<BudgetStatus>(src.Status)));

            CreateMap<Budget, BudgetDto>()
                .ForMember(d => d.Status,
                           o => o.MapFrom(src => src.Status.ToString()));
        }
    }
}
