using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.StoreLocation;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IStoreLocationService
    {

        Task<IEnumerable<StoreLocationDto>> GetStoreLocation();
        Task CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto);
        Task UpdateStoreLocation(int id, UpdateStoreLocationDto updateStoreLocationDto);
        Task<PagedViewModel<StoreLocationDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<StoreLocationDto> GetStoreById(int id);

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
        public async Task CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto)
        {
            var code = _configuration.GetValue<string>("Code:StoreLocation");

            createStoreLocationDto.IconPath = "icon_cup.png";

            var total = await _context.StoreLocations.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createStoreLocationDto.Code = code + codeLimit; }

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
                createStoreLocationDto.Code = code + s;

            }
            if (createStoreLocationDto.ImageStore != null)
            {
                string extension = Path.GetExtension(createStoreLocationDto.ImageStore.FileName);

                string image = Utilities.SEOUrl(createStoreLocationDto.Name) + extension;
                createStoreLocationDto.ImagePath = await Utilities.UploadFile(createStoreLocationDto.ImageStore, "ImageStore", image);

            }
            StoreLocation store = _mapper.Map<StoreLocation>(createStoreLocationDto);

            _context.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedViewModel<StoreLocationDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {


            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:StoreLocation"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.StoreLocations.Count() / (double)pageResult);
            var query = _context.StoreLocations.OrderBy(x => x.Id).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new StoreLocationDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Latitude = x.Latitude,
                                        Longitude = x.Longitude,
                                        Closed = x.Closed,
                                        Repaired = x.Repaired,
                                        ImagePath = x.ImagePath,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<StoreLocationDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }

        public async Task<StoreLocationDto> GetStoreById(int id)
        {
            var item = await _context.StoreLocations.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            return _mapper.Map<StoreLocationDto>(item);
        }

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocation()
        {
            var store = await _context.StoreLocations.OrderBy(x => x.Id).Where(x => x.Repaired == false && x.Closed == false).AsNoTracking().ToListAsync();

            var storeDto = _mapper.Map<List<StoreLocationDto>>(store);

            return storeDto;
        }

        public async Task UpdateStoreLocation(int id, UpdateStoreLocationDto updateStoreLocationDto)
        {
            var item = await _context.StoreLocations.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {

                if (updateStoreLocationDto.ImageStore != null)
                {
                    string extension = Path.GetExtension(updateStoreLocationDto.ImageStore.FileName);

                    string image = Utilities.SEOUrl(updateStoreLocationDto.Name) + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageSotre", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateStoreLocationDto.ImagePath = await Utilities.UploadFile(updateStoreLocationDto.ImageStore, "ImageStore", image);

                }


                _context.StoreLocations.Update(_mapper.Map(updateStoreLocationDto, item));

                await _context.SaveChangesAsync();


            }

            return;
        }
    }
}
