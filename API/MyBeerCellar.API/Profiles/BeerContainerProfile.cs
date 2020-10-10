using AutoMapper;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Profiles
{
    public class BeerContainerProfile : Profile
    {
        public BeerContainerProfile()
        {
            CreateMap<CreateBeerContainer, BeerContainer>()
                .ForMember(dest => dest.ContainerType,
                    opt => opt.MapFrom(src => src.ContainerType));

            CreateMap<UpdateBeerContainer, BeerContainer>()
                .ForMember(dest => dest.BeerContainerId,
                    opt => opt.MapFrom(src => src.BeerContainerId))
                .ForMember(dest => dest.ContainerType,
                    opt => opt.MapFrom(src => src.ContainerType));
        }
    }
}