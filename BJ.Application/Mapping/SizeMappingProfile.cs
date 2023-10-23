using AutoMapper;
using BJ.Contract.Size;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class SizeMappingProfile : Profile
    {
        public SizeMappingProfile()
        {
            CreateMap<Size, SizeDto>().ForPath(dest => dest.SizeSpecificProducts, opt => opt.MapFrom(src => src.SizeSpecificProducts));

            CreateMap<CreateSizeDto, Size>();


            CreateMap<UpdateSizeDto, Size>();

            CreateMap<SizeSpecificEachProduct, SizeSpecificProductDto>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Size.Price))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name))
                /*.ForPath(dest => dest.ProductDto, opt => opt.MapFrom(src => src.Product))*/;
            CreateMap<SizeSpecificEachProduct, UpdateSizeSpecificProductDto>();

            CreateMap<CreateSizeSpecificProductDto, SizeSpecificEachProduct>();


            CreateMap<UpdateSizeSpecificProductDto, SizeSpecificEachProduct>();
        }
    }
}
