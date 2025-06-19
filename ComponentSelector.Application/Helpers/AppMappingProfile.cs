using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Helpers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<Component, ComponentDto>();
        CreateMap<Component, ComponentWithCharacteristicsDto>().ReverseMap();
        CreateMap<Characteristic, CharacteristicDto>().ReverseMap();
        CreateMap<AppUser, UserDto>();
        // CreateMap<Build, BuildDto>();
        CreateMap<Build, UserBuildDto>();
        CreateMap<CreateUserBuildDto, Build>();
    }
}
