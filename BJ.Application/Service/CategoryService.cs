using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.SubCategory;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoryDtos();
        Task<IEnumerable<UserCategoryDto>> GetUserCategoryDtos();

        Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos();
        Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest);

        Task CreateCategory(CreateCategoryDto createCategoryDto);

        Task CreateSubCategory(CreateSubCategoryDto createSubCategoryDto);

        Task CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto);

        Task UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto);
        Task UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto);

        Task<CategoryDto> GetCategoryById(Guid id);
        Task<SubCategoryDto> GetSubCategoryById(int id);


    }
    public class CategoryService : ICategoryService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public CategoryService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.Id = Guid.NewGuid();

            createCategoryDto.Alias = Utilities.SEOUrl(createCategoryDto.CatName);

            var code = _configuration.GetValue<string>("Code:Category");

            createCategoryDto.Code = code + Utilities.GenerateStringDateTime();

            string extension = Path.GetExtension(createCategoryDto.Image.FileName);

            string image = Utilities.SEOUrl(createCategoryDto.CatName) + extension;

            createCategoryDto.ImagePath = await Utilities.UploadFile(createCategoryDto.Image, "ImageCategory", image);

            createCategoryDto.DateCreated = DateTime.Now;
            createCategoryDto.DateUpdated = DateTime.Now;
            Category category = _mapper.Map<Category>(createCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync();
        }

        public async Task CreateSubCategory(CreateSubCategoryDto createSubCategoryDto)
        {

            createSubCategoryDto.DateCreated = DateTime.Now;

            createSubCategoryDto.DateUpdated = DateTime.Now;

            string extension = Path.GetExtension(createSubCategoryDto.Image.FileName);

            string image = Utilities.SEOUrl(createSubCategoryDto.SubCatName) + extension;

            createSubCategoryDto.ImagePath = await Utilities.UploadFile(createSubCategoryDto.Image, "ImageSubCategory", image);


            SubCategory category = _mapper.Map<SubCategory>(createSubCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync();
        }

        public async Task CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto)
        {
            createSubCategorySpecificDto.Id = Guid.NewGuid();

            SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSubCategorySpecificDto);

            _context.Add(sizeSpecificEachProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            var cat = await _context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (cat == null) return null;

            return _mapper.Map<CategoryDto>(cat);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoryDtos()
        {
            var category = await _context.Categories.Include(x => x.Products.Where(x => x.BestSeller == true)).AsNoTracking().ToListAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(category);

            return categoryDto;
        }

        public async Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Category"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Categories.Count() / (double)pageResult);
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CatName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new CategoryDto()
                                    {
                                        Id = x.Id,
                                        CatName = x.CatName,
                                        Code = x.Code,
                                        Active = x.Active,
                                        ImagePath = x.ImagePath,
                                    }).ToListAsync();
            var categoryResponse = new PagedViewModel<CategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return categoryResponse;
        }
        public async Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:SubCategory"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.SubCategories.Count() / (double)pageResult);
            var query = _context.SubCategories.OrderBy(x => x.Id).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.SubCatName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new SubCategoryDto()
                                    {
                                        Id = x.Id,
                                        SubCatName = x.SubCatName,
                                        Active = x.Active,
                                        ImagePath = x.ImagePath,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<SubCategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }

        public async Task<SubCategoryDto> GetSubCategoryById(int id)
        {
            var subCat = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (subCat == null) return null;

            return _mapper.Map<SubCategoryDto>(subCat);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos()
        {
            var subCategory = await _context.SubCategories.AsNoTracking().ToListAsync();
            var subCategoryDto = _mapper.Map<List<SubCategoryDto>>(subCategory);

            return subCategoryDto;
        }

        public async  Task<IEnumerable<UserCategoryDto>> GetUserCategoryDtos()
        {
            var category = await _context.Categories.Include(x => x.Products).ThenInclude(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).AsNoTracking().ToListAsync();
            var categoryDto = _mapper.Map<List<UserCategoryDto>>(category);

            return categoryDto;
        }

        public async Task UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                updateCategoryDto.Alias = Utilities.SEOUrl(updateCategoryDto.CatName);

                if (updateCategoryDto.Image != null)
                {
                    string extension = Path.GetExtension(updateCategoryDto.Image.FileName);

                    string image = Utilities.SEOUrl(updateCategoryDto.CatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }
                    updateCategoryDto.ImagePath = await Utilities.UploadFile(updateCategoryDto.Image, "ImageCategory", image);

                }



                updateCategoryDto.DateUpdated = DateTime.Now;



                _context.Update(_mapper.Map(updateCategoryDto, item));

                await _context.SaveChangesAsync();
            }

        }

        public async Task UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto)
        {
            var item = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                if (updateSubCategoryDto.Image != null)
                {
                    string extension = Path.GetExtension(updateSubCategoryDto.Image.FileName);

                    string image = Utilities.SEOUrl(updateSubCategoryDto.SubCatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageSubCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }


                    updateSubCategoryDto.ImagePath = await Utilities.UploadFile(updateSubCategoryDto.Image, "ImageSubCategory", image);
                }


                updateSubCategoryDto.DateUpdated = DateTime.Now;

                _context.Update(_mapper.Map(updateSubCategoryDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
