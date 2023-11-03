using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.News;
using BJ.Contract.Translation.News;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface INewsService
    {

        Task<IEnumerable<NewsUserViewModel>> GetNewss(string culture, bool popular);
        Task CreateNews(CreateNewsAdminView createNewsAdminView);
        Task CreateNewsTranslate(CreateNewsTranslationDto createNewsTranslationDto);

        Task UpdateNews(Guid id, UpdateNewsAdminView updateNewsAdminView);
        Task<NewsUserViewModel> GetNewsById(Guid id, string culture);
        Task<PagedViewModel<NewsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<NewsTranslationDto> GetNewsTransalationById(Guid id);
        Task<IEnumerable<NewsUserViewModel>> GetNewsAtHome(string culture);

        Task CreateTranslateNews(CreateNewsTranslationDto createNewsTranslationDto);
        Task UpdateTranslateNews(Guid id, UpdateNewsTranslationDto updateNewsTranslationDto);

    }
    public class NewsService : INewsService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public NewsService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateNews(CreateNewsAdminView createNewsAdminView)
        {

            createNewsAdminView.CreateNews.Id = Guid.NewGuid();

            createNewsAdminView.CreateNews.DateUpdated = DateTime.Now;

            createNewsAdminView.CreateNews.DateCreated = DateTime.Now;


            using var transaction = _context.Database.BeginTransaction();

            if (createNewsAdminView.FileUpload != null)
            {
                string extension = Path.GetExtension(createNewsAdminView.FileUpload.FileName);

                string image = Utilities.SEOUrl(createNewsAdminView.CreateNewsTranslation.Title) + extension;

                createNewsAdminView.CreateNews.ImagePath = await Utilities.UploadFile(createNewsAdminView.FileUpload, "ImageNews", image);

            }
            var total = await _context.News.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var code = _configuration.GetValue<string>("Code:News");

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createNewsAdminView.CreateNews.Code = code + codeLimit; }

            else if (total.Count > 0)
            {
                var x = total[0].Code.Substring(code.Length, codeLimit.Length);

                int k = Convert.ToInt32(total[0].Code.Substring(code.Length, codeLimit.Length)) + 1;
                if (k < 10) s += total[0].Code.Substring(code.Length, codeLimit.Length - 1);
                else if (k < 100)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 2);
                else if (k < 1000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 3);
                else if (k < 10000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 4);
                else if (k < 100000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 5);
                else if (k < 1000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 6);
                else if (k < 10000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 7);
                else if (k < 100000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 8);
                else if (k < 1000000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 9);
                s += k.ToString();

                createNewsAdminView.CreateNews.Code = code + s;

            }
            News news = _mapper.Map<News>(createNewsAdminView.CreateNews);

            _context.Add(news);

            await _context.SaveChangesAsync();


            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            CreateNewsTranslationDto createNewsTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                NewsId = createNewsAdminView.CreateNews.Id,
                Title = createNewsAdminView.CreateNewsTranslation.Title,
                Alias = Utilities.SEOUrl(createNewsAdminView.CreateNewsTranslation.Title),
                Description = createNewsAdminView.CreateNewsTranslation.Description,
                ShortDesc = createNewsAdminView.CreateNewsTranslation.ShortDesc,
                MetaDesc = createNewsAdminView.CreateNewsTranslation.MetaDesc,
                MetaKey = createNewsAdminView.CreateNewsTranslation.MetaKey,
                LanguageId = defaultLanguage,

            };
            NewsTranslation poductTranslation = _mapper.Map<NewsTranslation>(createNewsTranslationDto);

            _context.Add(poductTranslation);

            await _context.SaveChangesAsync();


            await transaction.CommitAsync();

            return;

        }

        public async Task<PagedViewModel<NewsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:News"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.News.Count() / (double)pageResult);

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from n in _context.News.OrderByDescending(x => x.DateCreated)
                        join nt in _context.NewsTranslations on n.Id equals nt.NewsId
                        where nt.LanguageId == defaultLanguage && n.Id.Equals(nt.NewsId)
                        select new { n, nt };

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new NewsDto()
                                    {
                                        Code = x.n.Code,
                                        ImagePath = x.n.ImagePath,
                                        Active = x.n.Active,
                                        Popular = x.n.Popular,
                                        Title = x.nt.Title,
                                    }).AsNoTracking().ToListAsync();
            var newsResponse = new PagedViewModel<NewsDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return newsResponse;
        }



        public async Task<NewsUserViewModel> GetNewsById(Guid id, string culture)
        {
            var item = await _context.News.FindAsync(id);

            if (culture == null)
            {
                culture = _configuration.GetValue<string>("DefaultLanguageId");
            }

            var newsTranslation = await _context.NewsTranslations.FirstOrDefaultAsync(x => x.LanguageId == culture && x.NewsId.Equals(id));

            var newsViewModel = new NewsUserViewModel()
            {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Description = newsTranslation != null ? newsTranslation.Description : null,
                ShortDesc = newsTranslation != null ? newsTranslation.ShortDesc : null,
                Title = newsTranslation != null ? newsTranslation.Title : null,
                Alias = newsTranslation != null ? newsTranslation.Alias : null,
                MetaDesc = newsTranslation != null ? newsTranslation.MetaDesc : null,
                MetaKey = newsTranslation != null ? newsTranslation.MetaKey : null,
                DateUpdated = item.DateUpdated,
                Active = item.Active,
                Home = item.Home,
                ImagePath = item != null ? item.ImagePath : null,
                Popular = item.Popular,
                NewsTranslationDtos = _mapper.Map<List<NewsTranslationDto>>(await _context.NewsTranslations.Where(x => x.NewsId.Equals(id)).ToListAsync()),
            };
            return newsViewModel;

        }

        public async Task<IEnumerable<NewsUserViewModel>> GetNewss(string culture, bool popular)
        {
            var query = from b in _context.News
                        join bt in _context.NewsTranslations on b.Id equals bt.NewsId into ppic
                        from bt in ppic.DefaultIfEmpty()
                        where bt.LanguageId == culture && b.Active == true
                        select new { b, bt };
            var data = await query.OrderByDescending(x => x.b.DateCreated)
                .Select(x => new NewsUserViewModel()
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
                    Home = x.b.Home,

                    ImagePath = x.b.ImagePath,
                    Popular = x.b.Popular,
                }).ToListAsync();
            if (popular != false) { data = data.Where(x => x.Popular == popular).Take(10).ToList(); }
            return data;
        }

        public async Task UpdateNews(Guid id, UpdateNewsAdminView updateNewsAdminView)
        {
            var item = await _context.News.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                if (updateNewsAdminView.FileUpload != null)
                {
                    string extension = Path.GetExtension(updateNewsAdminView.FileUpload.FileName);

                    string image = Utilities.SEOUrl(updateNewsAdminView.UpdateNewsTranslation.Title) + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageNews", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateNewsAdminView.UpdateNews.ImagePath = await Utilities.UploadFile(updateNewsAdminView.FileUpload, "ImageNews", image);

                }


                updateNewsAdminView.UpdateNews.DateUpdated = DateTime.Now;

                _context.News.Update(_mapper.Map(updateNewsAdminView.UpdateNews, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.NewsTranslations.FirstOrDefaultAsync(x => x.NewsId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    var updateTranslate = new UpdateNewsTranslationDto()
                    {
                        Title = updateNewsAdminView.UpdateNewsTranslation.Title,
                        Description = updateNewsAdminView.UpdateNewsTranslation.Description,
                        ShortDesc = updateNewsAdminView.UpdateNewsTranslation.ShortDesc,
                        MetaDesc = updateNewsAdminView.UpdateNewsTranslation.MetaDesc,
                        MetaKey = updateNewsAdminView.UpdateNewsTranslation.MetaKey,
                        Alias = Utilities.SEOUrl(updateNewsAdminView.UpdateNewsTranslation.Title),


                    };
                    _context.Update(_mapper.Map(updateTranslate, translate));

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }

            return;
        }

        public async Task CreateNewsTranslate(CreateNewsTranslationDto createNewsTranslationDto)
        {
            createNewsTranslationDto.Id = Guid.NewGuid();
            createNewsTranslationDto.Alias = Utilities.SEOUrl(createNewsTranslationDto.Title);

            NewsTranslation transaltenews = _mapper.Map<NewsTranslation>(createNewsTranslationDto);

            _context.Add(transaltenews);

            await _context.SaveChangesAsync();
        }

        public async Task<NewsTranslationDto> GetNewsTransalationById(Guid id)
        {
            var newsTranslate = await _context.NewsTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var newsTranslateDto = _mapper.Map<NewsTranslationDto>(newsTranslate);

            return newsTranslateDto;
        }

        public async Task CreateTranslateNews(CreateNewsTranslationDto createNewsTranslationDto)
        {
            createNewsTranslationDto.Id = Guid.NewGuid();

            createNewsTranslationDto.Alias = Utilities.SEOUrl(createNewsTranslationDto.Title);


            NewsTranslation transaltenews = _mapper.Map<NewsTranslation>(createNewsTranslationDto);

            _context.Add(transaltenews);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTranslateNews(Guid id, UpdateNewsTranslationDto updateNewsTranslationDto)
        {
            var item = await _context.NewsTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));

            updateNewsTranslationDto.Alias = Utilities.SEOUrl(updateNewsTranslationDto.Title);

            if (item != null)
            {
                _context.Update(_mapper.Map(updateNewsTranslationDto, item));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NewsUserViewModel>> GetNewsAtHome(string culture)
        {
            var query = from b in _context.News
                        join bt in _context.NewsTranslations on b.Id equals bt.NewsId into ppic
                        from bt in ppic.DefaultIfEmpty()
                        where bt.LanguageId == culture && b.Active == true && b.Home == true
                        select new { b, bt };
            var data = await query.OrderByDescending(x => x.b.DateCreated)
                .Select(x => new NewsUserViewModel()
                {
                    Id = x.b.Id,
                    ShortDesc = x.bt.ShortDesc,
                    Title = x.bt.Title,
                    Alias = x.bt.Alias,
                    ImagePath = x.b.ImagePath,
                }).ToListAsync();
            return data.Take(1);
        }
    }
}
