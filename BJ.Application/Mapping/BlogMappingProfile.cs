using AutoMapper;
using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Blog, BlogDto>().ForPath(dest => dest.BlogTranslationDtos, opt => opt.MapFrom(src => src.BlogTranslations));


            CreateMap<CreateBlogDto, Blog>();


            CreateMap<UpdateBlogDto, Blog>();

            CreateMap<BlogTranslation, BlogTranslationDto>().ForPath(dest => dest.BlogDto, opt => opt.MapFrom(src => src.Blog));

            CreateMap<CreateBlogTranslationDto, BlogTranslation>();


            CreateMap<UpdateBlogTranslationDto, BlogTranslation>();

        }
    }
}
