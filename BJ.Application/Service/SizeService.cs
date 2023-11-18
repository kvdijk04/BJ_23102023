using AutoMapper;
using BJ.Application.Ultities;
using BJ.Contract.Size;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface ISizeService
    {

        Task<IEnumerable<SizeDto>> GetSizes();
        Task<IEnumerable<SizeDto>> GetSizesByCatId(Guid catId);

        Task<SizeSpecificProductDto> GetSize(int id, Guid productId);
        Task CreateSize(CreateSizeDto createSizeDto);
        Task UpdateSize(int id, UpdateSizeDto updateSizeDto);
        Task<SizeDto> GetSizeById(int id);
        Task<PagedViewModel<SizeDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class SizeService : ISizeService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public SizeService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateSize(CreateSizeDto createSizeDto)
        {
            createSizeDto.Updated = DateTime.Now;
            createSizeDto.Created = DateTime.Now;
            Size size = _mapper.Map<Size>(createSizeDto);

            _context.Add(size);
            await _context.SaveChangesAsync();

        }

        public async Task<PagedViewModel<SizeDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            //long ticks = DateTime.Now.Ticks;
            //byte[] bytes = BitConverter.GetBytes(ticks);
            //string id = Convert.ToBase64String(bytes);
            //var a = id.Replace('+', '_');
            //var b = a.Replace('/', '-');
            //var c = b.TrimEnd('=');


            //Console.WriteLine(id);


            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Size"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Sizes.Count() / (double)pageResult);
            var query = _context.Sizes.OrderBy(x => x.Id).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new SizeDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Active = x.Active,
                                        Note = x.Note,
                                        Price = x.Price,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<SizeDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }

        public async Task<SizeSpecificProductDto> GetSize(int id, Guid productId)
        {
            var size = await _context.SizeSpecificEachProduct.FirstOrDefaultAsync(x => x.SizeId == id && x.ProductId.Equals(productId));
            var sizeDto = _mapper.Map<SizeSpecificProductDto>(size);

            return sizeDto;
        }

        public async Task<SizeDto> GetSizeById(int id)
        {
            var item = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            return _mapper.Map<SizeDto>(item);

        }

        public async Task<IEnumerable<SizeDto>> GetSizes()
        {
            var size = await _context.Sizes.Include(x => x.SizeSpecificProducts).Where(x => x.Active == true).OrderByDescending(x => x.Created).AsNoTracking().ToListAsync();
            var sizeDto = _mapper.Map<List<SizeDto>>(size);
            return sizeDto;
        }

        public async Task<IEnumerable<SizeDto>> GetSizesByCatId(Guid catId)
        {
            var size = await _context.Sizes.Where(x => x.Active == true && x.CategoryId.Equals(catId)).OrderBy(x => x.Created).AsNoTracking().ToListAsync();
            var sizeDto = _mapper.Map<List<SizeDto>>(size);
            return sizeDto;
        }

        public async Task UpdateSize(int id, UpdateSizeDto updateSizeDto)
        {
            var item = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);

            if (item != null)
            {
                updateSizeDto.Updated = DateTime.Now;

                _context.Update(_mapper.Map(updateSizeDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
