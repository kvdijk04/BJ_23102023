using AutoMapper;
using BJ.Application.Helper;
using BJ.Contract.StoreLocation;
using BJ.Contract.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IStoreLocationService
    {

        Task<IEnumerable<StoreLocationDto>> GetStoreLocation();
        Task CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto);
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

            var total = await _context.StoreLocations.OrderBy(x => x.Id).AsNoTracking().ToListAsync();

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
            StoreLocation store = _mapper.Map<StoreLocation>(createStoreLocationDto);

            _context.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocation()
        {
            var store = await _context.StoreLocations.OrderBy(x => x.Id).Where(x => x.Repaired == false && x.Closed == false).AsNoTracking().ToListAsync();

            var storeDto = _mapper.Map<List<StoreLocationDto>>(store);

            return storeDto;
        }
    }
}
