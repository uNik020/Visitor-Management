using AutoMapper;
using VisitorManagement.DTO;
using VisitorManagement.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Visitor, VisitorReadDto>();
        CreateMap<VisitorCreateDto, Visitor>();
    }
}
