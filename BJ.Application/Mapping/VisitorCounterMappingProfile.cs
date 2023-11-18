using AutoMapper;
using BJ.Contract.VisitorCounter;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class VisitorCounterMappingProfile : Profile
    {
        public VisitorCounterMappingProfile()
        {
            CreateMap<VisitorCounter, VisitorCounterDto>();



            CreateMap<UpdateVisitorCounterDto, VisitorCounter>();


        }
    }
}
