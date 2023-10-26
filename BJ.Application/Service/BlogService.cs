using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IBlogService
    {

        Task<IEnumerable<BlogUserViewModel>> GetBlogs(string culture, bool popular);
        Task CreateBlog(CreateBlogAdminView createBlogAdminView);
        Task CreateBlogTranslate(CreateBlogTranslationDto createBlogTranslationDto);

        Task UpdateBlog(Guid id, UpdateBlogAdminView updateBlogAdminView);
        Task<BlogUserViewModel> GetBlogById(Guid id, string culture);
        Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<BlogTranslationDto> GetBlogTransalationById(Guid id);

        Task CreateTranslateBlog(CreateBlogTranslationDto createBlogTranslationDto);
        Task UpdateTranslateBlog(Guid id, UpdateBlogTranslationDto updateBlogTranslationDto);

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
            var code = _configuration.GetValue<string>("Code:Blog");

            createBlogAdminView.CreateBlog.Id = Guid.NewGuid();

            createBlogAdminView.CreateBlog.DateUpdated = DateTime.Now;

            createBlogAdminView.CreateBlog.DateCreated = DateTime.Now;


            using var transaction = _context.Database.BeginTransaction();

            if (createBlogAdminView.FileUpload != null)
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

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from b in _context.Blogs
                        join bt in _context.BlogTranslations on b.Id equals bt.BlogId into ppic
                        from bt in ppic.DefaultIfEmpty()
                        where bt.LanguageId == defaultLanguage
                        select new { b, bt };

            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new BlogDto()
                                    {
                                        Id = x.b.Id,
                                        ImagePath = x.b.ImagePath,
                                        DateCreated = x.b.DateCreated,
                                        Active = x.b.Active,
                                        DateUpdated = x.b.DateUpdated,
                                        Popular = x.b.Popular,
                                        Title = x.bt.Title,
                                    }).ToListAsync();
            var blogResponse = new PagedViewModel<BlogDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return blogResponse;
        }



        public async Task<BlogUserViewModel> GetBlogById(Guid id, string culture)
        {
            var item = await _context.Blogs.FindAsync(id);

            if(culture == null)
            {
                culture = _configuration.GetValue<string>("DefaultLanguageId");
            }

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
                BlogTranslationDtos = _mapper.Map<List<BlogTranslationDto>>(await _context.BlogTranslations.Where(x => x.BlogId.Equals(id)).ToListAsync()),
            };
            return blogViewModel;

        }

        public async Task<IEnumerable<BlogUserViewModel>> GetBlogs(string culture, bool popular)
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
            if (popular != false) { data = data.Where(x => x.Popular == popular).ToList(); }
            return data;
        }

        public async Task UpdateBlog(Guid id, UpdateBlogAdminView updateBlogAdminView)
        {
            var item = await _context.Blogs.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                if (updateBlogAdminView.FileUpload != null)
                {
                    string extension = Path.GetExtension(updateBlogAdminView.FileUpload.FileName);

                    string image = Utilities.SEOUrl(updateBlogAdminView.UpdateBlogTranslation.Title) + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageBlog", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateBlogAdminView.UpdateBlog.ImagePath= await Utilities.UploadFile(updateBlogAdminView.FileUpload, "ImageBlog", image);

                }


                updateBlogAdminView.UpdateBlog.DateUpdated = DateTime.Now;

                _context.Blogs.Update(_mapper.Map(updateBlogAdminView.UpdateBlog, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.BlogTranslations.FirstOrDefaultAsync(x => x.BlogId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    var updateTranslate = new UpdateBlogTranslationDto()
                    {
                        Title = updateBlogAdminView.UpdateBlogTranslation.Title,
                        Description = updateBlogAdminView.UpdateBlogTranslation.Description,
                        ShortDesc = updateBlogAdminView.UpdateBlogTranslation.ShortDesc,
                        MetaDesc = updateBlogAdminView.UpdateBlogTranslation.MetaDesc,
                        MetaKey = updateBlogAdminView.UpdateBlogTranslation.MetaKey,
                        Alias = Utilities.SEOUrl(updateBlogAdminView.UpdateBlogTranslation.Title),
                        

                    };
                    _context.Update(_mapper.Map(updateTranslate, translate));

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }

            return;
        }

        public async Task CreateBlogTranslate(CreateBlogTranslationDto createBlogTranslationDto)
        {
            createBlogTranslationDto.Id = Guid.NewGuid();
            createBlogTranslationDto.Alias = Utilities.SEOUrl(createBlogTranslationDto.Title);

            BlogTranslation transalteblog = _mapper.Map<BlogTranslation>(createBlogTranslationDto);

            _context.Add(transalteblog);

            await _context.SaveChangesAsync();
        }

        public async  Task<BlogTranslationDto> GetBlogTransalationById(Guid id)
        {
            var blogTranslate = await _context.BlogTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var blogTranslateDto = _mapper.Map<BlogTranslationDto>(blogTranslate);

            return blogTranslateDto;
        }

        public async Task CreateTranslateBlog(CreateBlogTranslationDto createBlogTranslationDto)
        {
            createBlogTranslationDto.Id = Guid.NewGuid();

            createBlogTranslationDto.Alias = Utilities.SEOUrl(createBlogTranslationDto.Title);

            
            BlogTranslation transalteblog = _mapper.Map<BlogTranslation>(createBlogTranslationDto);

            _context.Add(transalteblog);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTranslateBlog( Guid id, UpdateBlogTranslationDto updateBlogTranslationDto)
        {
            var item = await _context.BlogTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));

            updateBlogTranslationDto.Alias = Utilities.SEOUrl(updateBlogTranslationDto.Title);

            if (item != null)
            {
                _context.Update(_mapper.Map(updateBlogTranslationDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
