using AutoMapper;
using BJ.Contract.Size;
using BJ.Contract.StoreLocation;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            StoreLocation store = _mapper.Map<StoreLocation>(createStoreLocationDto);

            _context.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocation()
        {
            var store = await _context.StoreLocations.OrderBy(x => x.Id).AsNoTracking().ToListAsync();

            var storeDto = _mapper.Map<List<StoreLocationDto>>(store);

            return storeDto;
        }
    }
}
