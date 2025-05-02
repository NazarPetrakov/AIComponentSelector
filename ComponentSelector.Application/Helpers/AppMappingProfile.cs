using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Helpers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<Component, ComponentDto>();
        CreateMap<Component, ComponentWithCharacteristicsDto>();
        CreateMap<Characteristic, CharacteristicDto>();
        CreateMap<AppUser, UserDto>();
    }
}
