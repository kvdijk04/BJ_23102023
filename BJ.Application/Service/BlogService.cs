using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.Category;
using BJ.Contract.Translation.Product;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IBlogService
    {

        Task<IEnumerable<BlogUserViewModel>> GetBlogs(string culture,bool popular);
        Task CreateBlog(CreateBlogAdminView createBlogAdminView);
        Task CreateBlogTranslate(CreateBlogTranslationDto createBlogTranslationDto);

        Task UpdateBlog(Guid id, UpdateBlogDto updateBlogDto);
        Task<BlogUserViewModel> GetBlogById(Guid id, string culture);
        Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class BlogService : IBlogService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public BlogService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateBlog(CreateBlogAdminView createBlogAdminView)
        {
            var code = _configuration.GetValue<string>("Code:Product");

            createBlogAdminView.CreateBlog.Id = Guid.NewGuid();

            createBlogAdminView.CreateBlog.DateUpdated = DateTime.Now;

            createBlogAdminView.CreateBlog.DateCreated = DateTime.Now;


            using var transaction = _context.Database.BeginTransaction();

            if (createBlogAdminView.FileUpload!= null)
            {
                string extension = Path.GetExtension(createBlogAdminView.FileUpload.FileName);

                string image = Utilities.SEOUrl(createBlogAdminView.CreateBlogTranslation.Title) + extension;

                createBlogAdminView.CreateBlog.ImagePath = await Utilities.UploadFile(createBlogAdminView.FileUpload, "ImageBlog", image);

            }
            
            Blog blog = _mapper.Map<Blog>(createBlogAdminView.CreateBlog);

            _context.Add(blog);

            await _context.SaveChangesAsync();

            
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            CreateBlogTranslationDto createBlogTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                BlogId = createBlogAdminView.CreateBlog.Id,
                Title = createBlogAdminView.CreateBlogTranslation.Title,
                Alias = Utilities.SEOUrl(createBlogAdminView.CreateBlogTranslation.Title),
                Description = createBlogAdminView.CreateBlogTranslation.Description,
                ShortDesc = createBlogAdminView.CreateBlogTranslation.ShortDesc,
                MetaDesc = createBlogAdminView.CreateBlogTranslation.MetaDesc,
                MetaKey = createBlogAdminView.CreateBlogTranslation.MetaKey,
                LanguageId = defaultLanguage,
                
            };
            BlogTranslation poductTranslation = _mapper.Map<BlogTranslation>(createBlogTranslationDto);

            _context.Add(poductTranslation);

            await _context.SaveChangesAsync();


            await transaction.CommitAsync();

            return;

        }

        public async Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Blog"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Blogs.Count() / (double)pageResult);
            var query = _context.Blogs.OrderBy(x => x.Id).AsNoTracking().AsQueryable();
            //if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            //{
            //    query = query.Where(x => x.tit.Contains(getListPagingRequest.Keyword));
            //    pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            //}


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new BlogDto()
                                    {
                                        Id = x.Id,
                                        ImagePath = x.ImagePath,
                                        DateCreated = x.DateCreated,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<BlogDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }



        public async Task<BlogUserViewModel> GetBlogById(Guid id, string culture)
        {
            var item = await _context.Blogs.FindAsync(id);

            var blogTranslation = await _context.BlogTranslations.FirstOrDefaultAsync(x => x.LanguageId == culture && x.BlogId.Equals(id));

            var blogViewModel = new BlogUserViewModel()
            {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Description = blogTranslation != null ? blogTranslation.Description : null,
                ShortDesc = blogTranslation != null ? blogTranslation.ShortDesc : null,
                Title = blogTranslation != null ? blogTranslation.Title : null,
                Alias = blogTranslation != null ? blogTranslation.Alias : null,
                MetaDesc = blogTranslation != null ? blogTranslation.MetaDesc : null,
                MetaKey = blogTranslation != null ? blogTranslation.MetaKey : null,
                DateUpdated = item.DateUpdated,
                Active = item.Active,
                ImagePath = item != null ? item.ImagePath : null,
                Popular = item.Popular,
            };
            return blogViewModel;

        }

        public async Task<IEnumerable<BlogUserViewModel>> GetBlogs(string culture,bool popular)
        {
            var query = from b in _context.Blogs
                        join bt in _context.BlogTranslations on b.Id equals bt.BlogId into ppic
                        from bt in ppic.DefaultIfEmpty()
                        where bt.LanguageId == culture && b.Active == true
                        select new { b, bt };
            var data = await query.OrderByDescending(x => x.b.DateCreated)
                .Select(x => new BlogUserViewModel()
                {
                    Id = x.b.Id,
                    DateCreated = x.b.DateCreated,
                    Description = x.bt.Description,
                    ShortDesc = x.bt.ShortDesc,
                    Title = x.bt.Title,
                    Alias = x.bt.Alias,
                    MetaDesc = x.bt.MetaDesc,
                    MetaKey = x.bt.MetaKey,
                    DateUpdated = x.b.DateUpdated,
                    Active = x.b.Active,
                    ImagePath = x.b.ImagePath,
                    Popular = x.b.Popular,
                }).ToListAsync();
            if(popular != false) { data = data.Where(x => x.Popular == popular).ToList(); }
            return data;
        }

        public async Task UpdateBlog(Guid id, UpdateBlogDto updateBlogDto)
        {
            var item = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

            if (item != null)
            {
                updateBlogDto.DateUpdated = DateTime.Now;

                _context.Update(_mapper.Map(updateBlogDto, item));

                await _context.SaveChangesAsync();
            }
        }

        public async  Task CreateBlogTranslate(CreateBlogTranslationDto createBlogTranslationDto)
        {
            createBlogTranslationDto.Id = Guid.NewGuid();
            createBlogTranslationDto.Alias = Utilities.SEOUrl(createBlogTranslationDto.Title);

            BlogTranslation transaltecategory = _mapper.Map<BlogTranslation>(createBlogTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync();
        }
    }
}
