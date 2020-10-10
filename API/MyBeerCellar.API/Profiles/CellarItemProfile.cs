using AutoMapper;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Profiles
{
    public class CellarItemProfile : Profile
    {
        public CellarItemProfile()
        {
            CreateMap<CreateCellarItem, CellarItem>()
                .ForMember(dest => dest.BeerContainerId,
                    opt => opt.MapFrom(src => src.BeerContainerId))
                .ForMember(dest => dest.BeerStyleId,
                    opt => opt.MapFrom(src => src.BeerStyleId))
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.YearProduced,
                    opt => opt.MapFrom(src => src.YearProduced));

            CreateMap<UpdateCellarItem, CellarItem>()
                .ForMember(dest => dest.BeerContainerId,
                    opt => opt.MapFrom(src => src.BeerContainerId))
                .ForMember(dest => dest.BeerStyleId,
                    opt => opt.MapFrom(src => src.BeerStyleId))
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.YearProduced,
                    opt => opt.MapFrom(src => src.YearProduced));
        }
    }
}
