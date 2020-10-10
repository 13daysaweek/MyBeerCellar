using AutoMapper;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Profiles
{
    public class BeerStyleProfile : Profile
    {
        public BeerStyleProfile()
        {
            CreateMap<CreateBeerStyle, BeerStyle>()
                .ForMember(dest => dest.StyleName,
                    opt => opt.MapFrom(src => src.StyleName));

            CreateMap<UpdateBeerStyle, BeerStyle>()
                .ForMember(dest => dest.StyleId,
                    opt => opt.MapFrom(src => src.StyleId))
                .ForMember(dest => dest.StyleName,
                    opt => opt.MapFrom(src => src.StyleName));
        }
    }
}
