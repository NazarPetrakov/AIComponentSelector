using AutoMapper;
using ComponentSelector.Application.DTOs;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Helpers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterDto, AppUser>();
    }
}
