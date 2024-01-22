using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.StoreLocation;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Store;
using BJ.Contract.Translation.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IStoreLocationService
    {

        Task<IEnumerable<StoreLocationDto>> GetStoreLocation(string languageId);
        Task CreateStoreLocation(CreateStoreLocationAdminView createStoreLocationAdminView);
        Task CreateStoreLocationTimeline(CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto);

        Task UpdateStoreLocation(int id, UpdateStoreLocationAdminView updateStoreLocationAdminView);
        Task UpdateStoreLocationTimeLine(int id, UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto);

        Task<PagedViewModel<StoreLocationDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<StoreLocationDto> GetStoreById(int id);
        Task<StoreLocationOpenHourDto> GetStoreLocationTimeLineById(int id);
        Task<StoreLocationTranslationDto> GetStoreLocationTranslatebyId(Guid id);
        Task CreateTranslateStoreLocation(CreateStoreLocationTranslationDto createStoreLocationTranslationDto);
        Task UpdateTranslateStoreLocation(Guid id, UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto);

    }
    public class StoreLocationService : IStoreLocationService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public StoreLocationService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task CreateStoreLocation(CreateStoreLocationAdminView createStoreLocationAdminView)
        {
            using var transaction = _context.Database.BeginTransaction();

            var code = _configuration.GetValue<string>("Code:StoreLocation");

            createStoreLocationAdminView.CreateStoreLocationDto.IconPath = "icon_cup.png";

            var total = await _context.StoreLocations.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createStoreLocationAdminView.CreateStoreLocationDto.Code = code + codeLimit; }

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

                createStoreLocationAdminView.CreateStoreLocationDto.Code = code + s;

            }
            if (createStoreLocationAdminView.ImageStore != null)
            {
                string extension = Path.GetExtension(createStoreLocationAdminView.ImageStore.FileName);

                string image = Utilities.SEOUrl(createStoreLocationAdminView.CreateStoreLocationTranslation.Name) + extension;

                createStoreLocationAdminView.CreateStoreLocationDto.ImagePath = await Utilities.UploadFile(createStoreLocationAdminView.ImageStore, "ImageStore", image);

            }
            if (createStoreLocationAdminView.CreateStoreLocationDto.Sort == null)
            {
                var currentSort = await _context.StoreLocations.OrderByDescending(x => x.Sort).AsNoTracking().ToListAsync();

                createStoreLocationAdminView.CreateStoreLocationDto.Sort = currentSort[0].Sort + 1;
            }
            else { if (_context.StoreLocations.Any(x => x.Sort == createStoreLocationAdminView.CreateStoreLocationDto.Sort) == true) { return; } }

            StoreLocation store = _mapper.Map<StoreLocation>(createStoreLocationAdminView.CreateStoreLocationDto);

            _context.Add(store);

            await _context.SaveChangesAsync(createStoreLocationAdminView.CreateStoreLocationDto.UserName);

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");
            createStoreLocationAdminView.CreateStoreLocationTranslation.Id = Guid.NewGuid();

            createStoreLocationAdminView.CreateStoreLocationTranslation.StoreLocationId = store.Id;

            createStoreLocationAdminView.CreateStoreLocationTranslation.LanguageId = defaultLanguage;

            createStoreLocationAdminView.CreateStoreLocationTranslation.DateCreated = DateTime.Now;

            StoreLocationTranslation storeLocationTranslation = _mapper.Map<StoreLocationTranslation>(createStoreLocationAdminView.CreateStoreLocationTranslation);

            _context.Add(storeLocationTranslation);

            await _context.SaveChangesAsync(createStoreLocationAdminView.CreateStoreLocationDto.UserName);

            await transaction.CommitAsync();
        }

        public async Task CreateStoreLocationTimeline(CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto)
        {

            createStoreLocationTimeLineDto.DateCreated = DateTime.Now;


            StoreLocationOpenHour openHour = _mapper.Map<StoreLocationOpenHour>(createStoreLocationTimeLineDto);

            if (_context.StoreLocationOpenHours.Any(x => x.StoreLocationId == openHour.StoreLocationId && x.DaysOfWeek == openHour.DaysOfWeek) == true)
            {
                return;
            }
            _context.Add(openHour);

            await _context.SaveChangesAsync(createStoreLocationTimeLineDto.UserName);
        }

        public async Task CreateTranslateStoreLocation(CreateStoreLocationTranslationDto createStoreLocationTranslationDto)
        {
            var exist = _context.StoreLocationTranslations.Any(x => x.StoreLocationId.Equals(createStoreLocationTranslationDto.StoreLocationId) && x.LanguageId == createStoreLocationTranslationDto.LanguageId);

            if (exist) return;

            createStoreLocationTranslationDto.Id = Guid.NewGuid();

            createStoreLocationTranslationDto.DateCreated = DateTime.Now;

            StoreLocationTranslation transaltecategory = _mapper.Map<StoreLocationTranslation>(createStoreLocationTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync(createStoreLocationTranslationDto.UserName);
        }

        public async Task<PagedViewModel<StoreLocationDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {


            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:StoreLocation"));
            }

            var pageResult = getListPagingRequest.PageSize;

            getListPagingRequest.LanguageId = _configuration.GetValue<string>("DefaultLanguageId");


            var pageCount = Math.Ceiling(_context.StoreLocations.Count() / (double)pageResult);
            var query = from sl in _context.StoreLocations.Include(x => x.StoreLocationOpenHours.Where(x => x.Active == true).OrderBy(x => x.Id)).OrderBy(x => x.Sort)
                        join slt in _context.StoreLocationTranslations on sl.Id equals slt.StoreLocationId into sll
                        from slt in sll.DefaultIfEmpty()
                        where slt.LanguageId == getListPagingRequest.LanguageId /*&& sloh.Active == true */
                        select new { sl, slt };
            //if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            //{
            //    query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
            //    pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            //}


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new StoreLocationDto()
                                    {
                                        Id = x.sl.Id,
                                        Name = x.slt.Name,
                                        Closed = x.sl.Closed,
                                        Code = x.sl.Code,
                                        IconPath = x.sl.IconPath,
                                        ImagePath = x.sl.ImagePath,
                                        OpeningSoon = x.sl.OpeningSoon,
                                        Repaired = x.sl.Repaired,
                                        Sort = x.sl.Sort,
                                    }).ToListAsync();
            var subStoreLocationResponse = new PagedViewModel<StoreLocationDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subStoreLocationResponse;
        }

        public async Task<StoreLocationDto> GetStoreById(int id)
        {
            var item = await _context.StoreLocations.Include(x => x.StoreLocationOpenHours).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var storeTranslation = await _context.StoreLocationTranslations.FirstOrDefaultAsync(x => x.LanguageId == culture && x.StoreLocationId == id);

            var storeDto = new StoreLocationDto()
            {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Name = storeTranslation != null ? storeTranslation.Name : null,
                Address = storeTranslation != null ? storeTranslation.Address : null,
                Code = item.Code,
                DateUpdated = item.DateUpdated,
                OpeningSoon = item.OpeningSoon,
                Closed = item.Closed,
                Repaired = item.Repaired,
                Longitude = item.Longitude,
                Latitude = item.Latitude,   
                Sort = item.Sort,
                City = storeTranslation.City,
                IconPath = item.IconPath,
                ImagePath = item != null ? item.ImagePath : null,
                StoreLocationTranslationDtos = _mapper.Map<List<StoreLocationTranslationDto>>
                (await _context.StoreLocationTranslations.Where(x => x.StoreLocationId == id).Select(x => new StoreLocationTranslationDto
                {
                    LanguageId = x.LanguageId,
                    Id = x.Id,
                    Name = x.Name, Address = x.Address,City = x.City,

                }).ToListAsync()),
                StoreLocationOpenHourDtos = _mapper.Map<List<StoreLocationOpenHourDto>>(item.StoreLocationOpenHours),

            };
            return storeDto;
           
        }

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocation(string languageId)
        {
            if (languageId == null) languageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from sl in _context.StoreLocations.Include(x => x.StoreLocationOpenHours.Where(x => x.Active == true).OrderBy(x => x.Id)).OrderBy(x => x.Sort).Where(x => x.Closed == false && x.Repaired == false || x.Repaired == true && x.Closed == false || x.OpeningSoon == true && x.Closed == false)
                        join slt in _context.StoreLocationTranslations on sl.Id equals slt.StoreLocationId into sll
                        from slt in sll.DefaultIfEmpty()
                        where slt.LanguageId == languageId /*&& sloh.Active == true */
                        select new { sl, slt };
            var storeDto = await query.Select(x => new StoreLocationDto()
            {
                Id = x.sl.Id,
                Name = x.slt.Name,
                Address = x.slt.Address,
                City = x.slt.City,
                Closed = x.sl.Closed,
                Code = x.sl.Code,
                DateCreated = x.sl.DateCreated,
                DateUpdated = x.sl.DateUpdated,
                IconPath = x.sl.IconPath,
                ImagePath = x.sl.ImagePath,
                Latitude = x.sl.Latitude,
                Longitude = x.sl.Longitude,
                OpeningSoon = x.sl.OpeningSoon,
                Repaired = x.sl.Repaired,
                Sort = x.sl.Sort,
                StoreLocationOpenHourDtos = _mapper.Map<List<StoreLocationOpenHourDto>>(x.sl.StoreLocationOpenHours),

            }).ToListAsync();



            return storeDto;
        }

        public async Task<StoreLocationOpenHourDto> GetStoreLocationTimeLineById(int id)
        {
            var item = await _context.StoreLocationOpenHours.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item == null) return null;
            return _mapper.Map<StoreLocationOpenHourDto>(item);
        }

        public async Task<StoreLocationTranslationDto> GetStoreLocationTranslatebyId(Guid id)
        {
            var storeTranslate = await _context.StoreLocationTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var storeTranslateDto = _mapper.Map<StoreLocationTranslationDto>(storeTranslate);

            return storeTranslateDto;
        }

        public async Task UpdateStoreLocation(int id, UpdateStoreLocationAdminView updateStoreLocationAdminView)
        {
            var item = await _context.StoreLocations.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                if (updateStoreLocationAdminView.ImageStore != null)
                {
                    string extension = Path.GetExtension(updateStoreLocationAdminView.ImageStore.FileName);

                    string image = Utilities.SEOUrl(updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Name) + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageSotre", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath = await Utilities.UploadFile(updateStoreLocationAdminView.ImageStore, "ImageStore", image);

                }


                updateStoreLocationAdminView.UpdateStoreLocationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateStoreLocationAdminView.UpdateStoreLocationDto);


                await _context.SaveChangesAsync(updateStoreLocationAdminView.UpdateStoreLocationDto.UserName);

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.StoreLocationTranslations.FirstOrDefaultAsync(x => x.StoreLocationId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {

                    updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.DateUpdated = DateTime.Now;

                    _context.Entry(translate).CurrentValues.SetValues(updateStoreLocationAdminView.UpdateStoreLocationTranslationDto);

                    await _context.SaveChangesAsync(updateStoreLocationAdminView.UpdateStoreLocationDto.UserName);
                }




                await transaction.CommitAsync();
            }

            return;
        }

        public async Task UpdateStoreLocationTimeLine(int id, UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto)
        {

            updateStoreLocationTimeLineDto.DateUpdated = DateTime.Now;

            var item = await _context.StoreLocationOpenHours.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {


                updateStoreLocationTimeLineDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateStoreLocationTimeLineDto);

                await _context.SaveChangesAsync(updateStoreLocationTimeLineDto.UserName);

            }
            return;
        }

        public async Task UpdateTranslateStoreLocation(Guid id, UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto)
        {
            var item = await _context.StoreLocationTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {
                updateStoreLocationTranslationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateStoreLocationTranslationDto);

                await _context.SaveChangesAsync(updateStoreLocationTranslationDto.UserName);

            }
        }
    }
}
