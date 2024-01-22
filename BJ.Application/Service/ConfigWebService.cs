using AutoMapper;
using BJ.Application.Ultities;
using BJ.Contract.Account;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IConfigWebService
    {
        Task<IEnumerable<ConfigWebDto>> GetConfigWebs();
        Task CreateConfigWeb(CreateConfigWebDto createConfigWebDto);
        Task UpdateConfigWeb(int id, UpdateConfigWebDto updateConfigWebDto);
        Task<ConfigWebDto> GetConfigWebById(int id);
        Task<PagedViewModel<ConfigWebDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class ConfigWebService : IConfigWebService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public ConfigWebService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateConfigWeb(CreateConfigWebDto createConfigWebDto)
        {
            createConfigWebDto.DateCreated = DateTime.Now;

            ConfigWebsite size = _mapper.Map<ConfigWebsite>(createConfigWebDto);

            _context.Add(size);
            await _context.SaveChangesAsync(createConfigWebDto.UserName);

        }

        public async Task<PagedViewModel<ConfigWebDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:ConfigWeb"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.ConfigWebs.Count() / (double)pageResult);
            var query = _context.ConfigWebs.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new ConfigWebDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,

                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<ConfigWebDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }



        public async Task<ConfigWebDto> GetConfigWebById(int id)
        {
            var item = await _context.ConfigWebs.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item == null) return null;
            return _mapper.Map<ConfigWebDto>(item);

        }

        public async Task<IEnumerable<ConfigWebDto>> GetConfigWebs()
        {
            var size = await _context.ConfigWebs.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var sizeDto = _mapper.Map<List<ConfigWebDto>>(size);
            return sizeDto;
        }


        public async Task UpdateConfigWeb(int id, UpdateConfigWebDto updateConfigWebDto)
        {
            var item = await _context.ConfigWebs.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {
                updateConfigWebDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateConfigWebDto);

                await _context.SaveChangesAsync(updateConfigWebDto.UserName);
            }
        }
    }
}
