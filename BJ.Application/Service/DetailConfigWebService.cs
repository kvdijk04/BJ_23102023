using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Contract.Product;
using BJ.Contract.StoreLocation;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.ConfigWeb;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace BJ.Application.Service
{
    public interface IDetailConfigWebService
    {
        Task<IEnumerable<ConfigWebViewModel>> GetDetailConfigWebs(string culture);
        Task<ApiResult> CreateDetailConfigWeb(CreateConfigWebAdminView createConfigWebAdminView);
        Task<ApiResult> UpdateDetailConfigWeb(Guid id, UpdateDetailConfigWebDto updateDetailConfigWebDto);
        Task<ConfigWebViewModel> GetDetailConfigWebById(Guid id,string culture);
        Task<ConfigWebViewModel> GetDetailConfigWebByUrl(string url, string culture);

        Task<PagedViewModel<ConfigWebViewModel>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<DetailConfigWebTranslationDto> GetDetailConfigWebTranslationById(Guid id);

        Task CreateTranslateDetailConfigWeb(CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto);
        Task UpdateTranslateDetailConfigWeb(Guid id, UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto);
    }
    public class DetailConfigWebService : IDetailConfigWebService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public DetailConfigWebService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResult> CreateDetailConfigWeb(CreateConfigWebAdminView createDetailConfigWebDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            var sortCurrent = await _context.DetailConfigWebsites.Where(x => x.ConfigWebId == createDetailConfigWebDto.CreateDetailConfigWebDto.ConfigWebId).OrderByDescending(x => x.SortOrder).ToListAsync();

            ApiResult apiResult = new();

            if(createDetailConfigWebDto.AutoSort == true)
            {
                if (sortCurrent.Count > 0)
                {
                    createDetailConfigWebDto.CreateDetailConfigWebDto.SortOrder = sortCurrent[0].SortOrder + 1;

                }
                else
                {
                    createDetailConfigWebDto.CreateDetailConfigWebDto.SortOrder = 1;

                }
            }

            if(createDetailConfigWebDto.NewPage == true &&  _context.DetailConfigWebsiteTranslations.Any(x => x.Url == createDetailConfigWebDto.CreateDetailConfigWebTranslationDto.Url) == true)
            {
                apiResult.Msg = "Địa chỉ web đã được sử dụng";
                return apiResult;
            }

            createDetailConfigWebDto.CreateDetailConfigWebDto.Id = Guid.NewGuid();

            createDetailConfigWebDto.CreateDetailConfigWebDto.DateCreated = DateTime.Now;

            DetailConfigWebsite detailConfigWeb = _mapper.Map<DetailConfigWebsite>(createDetailConfigWebDto.CreateDetailConfigWebDto);

            _context.Add(detailConfigWeb);

            await _context.SaveChangesAsync(createDetailConfigWebDto.CreateDetailConfigWebDto.UserName);

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                DetailConfigWebId = createDetailConfigWebDto.CreateDetailConfigWebDto.Id,
                Title = createDetailConfigWebDto.CreateDetailConfigWebTranslationDto.Title,
                Description = createDetailConfigWebDto.CreateDetailConfigWebTranslationDto.Description,
                Url = createDetailConfigWebDto.CreateDetailConfigWebTranslationDto.Url,
                LanguageId = defaultLanguage,

            };
            DetailConfigWebsiteTranslation detailConfigWebTranslation = _mapper.Map<DetailConfigWebsiteTranslation>(createDetailConfigWebTranslationDto);

            _context.Add(detailConfigWebTranslation);

            await _context.SaveChangesAsync(createDetailConfigWebDto.CreateDetailConfigWebDto.UserName);


            await transaction.CommitAsync();

            apiResult.Msg = "OK";
            return apiResult;

        }

        public async Task<PagedViewModel<ConfigWebViewModel>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            if (getListPagingRequest.PageSize == 0) { getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:DetailConfigWeb")); }

            var pageResult = getListPagingRequest.PageSize;

            var pageCount = Math.Ceiling(_context.DetailConfigWebsites.Count() / (double)pageResult);

            if(getListPagingRequest.LanguageId == null) { getListPagingRequest.LanguageId = _configuration.GetValue<string>("DefaultLanguageId"); }

            var query = from dcw in _context.DetailConfigWebsites.OrderByDescending(x => x.DateCreated).AsNoTracking().AsSingleQuery()
                           join cw in _context.ConfigWebs on dcw.ConfigWebId equals cw.Id into cwd
                           from cw in cwd.DefaultIfEmpty()
                           join dcwt in _context.DetailConfigWebsiteTranslations on dcw.Id equals dcwt.DetailConfigWebId

                           where dcwt.LanguageId == getListPagingRequest.LanguageId
                           select new { dcw, cw, dcwt};


            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.dcwt.Title.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.CategoryId > 0)
            {
                query = query.Where(x => x.dcw.ConfigWebId == getListPagingRequest.CategoryId);
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);

            }

            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new ConfigWebViewModel()
                                    {
                                        Id = x.dcw.Id,
                                        NameConfig = x.cw.Name,
                                        DateCreated = x.dcw.DateCreated,
                                        DateUpdated = x.dcw.DateUpdated,
                                        Description = x.dcwt.Description,
                                        Title = x.dcwt.Title,
                                        LanguageId = x.dcwt.LanguageId,
                                        Active = x.dcw.Active,
                                        SortOrder = x.dcw.SortOrder,
                                        Url = x.dcwt.Url,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<ConfigWebViewModel>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }



        public async Task<ConfigWebViewModel> GetDetailConfigWebById(Guid id,string culture)
        {
            if (culture == null) { culture = _configuration.GetValue<string>("DefaultLanguageId"); }

            var detail = await _context.DetailConfigWebsites.Include(x => x.ConfigWeb).Include(x => x.DetailConfigWebTranslations).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var detailTranslate = await _context.DetailConfigWebsiteTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.DetailConfigWebId.Equals(id) && x.LanguageId == culture);

            var configWebDto = new ConfigWebViewModel()
            {
                Id = id,
                ConfigId = detail.ConfigWebId,
                DateCreated = detail.DateCreated,
                DateUpdated = detail.DateUpdated,
                Description = detailTranslate.Description,
                LanguageId = detailTranslate.LanguageId,
                Title = detailTranslate.Title,
                NameConfig = detail.ConfigWeb.Name,
                Url = detailTranslate.Url,
                Active = detail.Active,
                SortOrder = detail.SortOrder,
                DetailConfigWebTranslationDtos = _mapper.Map<List<DetailConfigWebTranslationDto>>(detail.DetailConfigWebTranslations),
            };

            return configWebDto;

        }

        public async Task<IEnumerable<ConfigWebViewModel>> GetDetailConfigWebs(string culture)
        {
            var query = from dcw in _context.DetailConfigWebsites.OrderByDescending(x => x.DateCreated).AsNoTracking().AsSingleQuery()
                        join cw in _context.ConfigWebs on dcw.ConfigWebId equals cw.Id into cwd
                        from cw in cwd.DefaultIfEmpty()
                        join dcwt in _context.DetailConfigWebsiteTranslations on dcw.Id equals dcwt.DetailConfigWebId

                        where dcwt.LanguageId == culture
                        select new { dcw, cw, dcwt };

            var configViewModel = await query.Select(x => new ConfigWebViewModel()
            {
                Id = x.dcw.Id,
                NameConfig = x.cw.Name,
                DateCreated = x.dcw.DateCreated,
                DateUpdated = x.dcw.DateUpdated,
                Title = x.dcwt.Title,
                LanguageId = x.dcwt.LanguageId,
                Url = x.dcwt.Url,
                Active = x.dcw.Active,
                ConfigId = x.dcw.ConfigWebId,
                SortOrder = x.dcw.SortOrder, 
            }).OrderBy(x => x.SortOrder).AsNoTracking().ToListAsync();
            return configViewModel;
        }


        public async Task<ApiResult> UpdateDetailConfigWeb(Guid id, UpdateDetailConfigWebDto updateDetailConfigWebDto)
        {
            var item = await _context.DetailConfigWebsites.FirstOrDefaultAsync(x => x.Id.Equals(id));

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var itemTranslate = await _context.DetailConfigWebsiteTranslations.FirstOrDefaultAsync(x => x.DetailConfigWebId.Equals(id) && x.LanguageId == defaultLanguage);

            using var transaction = _context.Database.BeginTransaction();

            ApiResult apiResult = new();

            if (item != null && itemTranslate != null)
            {
                if (updateDetailConfigWebDto.SortOrder == 0)
                {
                    var currentSort = await _context.DetailConfigWebsites.OrderByDescending(x => x.SortOrder).AsNoTracking().ToListAsync();

                    updateDetailConfigWebDto.SortOrder = currentSort[1].SortOrder + 1;
                }
                else { if (_context.DetailConfigWebsites.Any(x => x.SortOrder == updateDetailConfigWebDto.SortOrder) == true) {
                        apiResult.Msg = "Thứ tự này dã được đặt";
                        return apiResult;
                } }


                updateDetailConfigWebDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateDetailConfigWebDto);

                await _context.SaveChangesAsync(updateDetailConfigWebDto.UserName);

                if (updateDetailConfigWebDto.NewPage == true && updateDetailConfigWebDto.Url != itemTranslate.Url && _context.DetailConfigWebsiteTranslations.Any(x => x.Url == updateDetailConfigWebDto.Url) == true)
                {
                    apiResult.Msg = "Địa chỉ web đã được sử dụng";
                    return apiResult;
                }

                var updateTranslate = new UpdateDetailConfigWebTranslationDto()
                {
                    Title = updateDetailConfigWebDto.Title,
                    Description = updateDetailConfigWebDto.Description,
                    Url = updateDetailConfigWebDto.Url,
                };
                _context.Entry(itemTranslate).CurrentValues.SetValues(updateTranslate);

                await _context.SaveChangesAsync(updateDetailConfigWebDto.UserName);

                await transaction.CommitAsync();


            }
            apiResult.Msg = "OK";

            return apiResult;
        }

        public async Task<ConfigWebViewModel> GetDetailConfigWebByUrl(string url, string culture)
        {
            var detailTranslate = await _context.DetailConfigWebsiteTranslations.FirstOrDefaultAsync(x => x.Url.Equals(url) && x.LanguageId == culture);
            var detail = await _context.DetailConfigWebsites.Include(x => x.ConfigWeb).FirstOrDefaultAsync(x => x.Id.Equals(detailTranslate.DetailConfigWebId));

            var configWebDto = new ConfigWebViewModel()
            {
                Id = detail.Id,
                ConfigId = detail.ConfigWebId,
                DateCreated = detail.DateCreated,
                DateUpdated = detail.DateUpdated,
                Description = detailTranslate.Description,
                LanguageId = detailTranslate.LanguageId,
                Title = detailTranslate.Title,
                NameConfig = detail.ConfigWeb.Name,
                Url = detailTranslate.Url,
                Active = detail.Active,
                SortOrder = detail.SortOrder,
            };

            return configWebDto;
        }

        public async  Task<DetailConfigWebTranslationDto> GetDetailConfigWebTranslationById(Guid id)
        {
            var detailConfigWebTranslate = await _context.DetailConfigWebsiteTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var detailConfigWebTranslateDto = _mapper.Map<DetailConfigWebTranslationDto>(detailConfigWebTranslate);

            return detailConfigWebTranslateDto;
        }

        public async Task CreateTranslateDetailConfigWeb(CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto)
        {
            var exist = _context.DetailConfigWebsiteTranslations.Any(x => x.DetailConfigWebId.Equals(createDetailConfigWebTranslationDto.DetailConfigWebId) && x.LanguageId == createDetailConfigWebTranslationDto.LanguageId);
            if (exist) return;

            createDetailConfigWebTranslationDto.Id = Guid.NewGuid();

            createDetailConfigWebTranslationDto.DateCreated  = DateTime.Now;
            DetailConfigWebsiteTranslation detailConfigWeb = _mapper.Map<DetailConfigWebsiteTranslation>(createDetailConfigWebTranslationDto);

            _context.Add(detailConfigWeb);

            await _context.SaveChangesAsync(createDetailConfigWebTranslationDto.UserName);
        }

        public async Task UpdateTranslateDetailConfigWeb(Guid id, UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto)
        {
            var item = await _context.DetailConfigWebsiteTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                updateDetailConfigWebTranslationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateDetailConfigWebTranslationDto);

                await _context.SaveChangesAsync(updateDetailConfigWebTranslationDto.UserName);
            }
        }
    }
}
