using AutoMapper;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IConfigProductService
    {
        Task<SizeSpecificProductDto> GetSizeSpecificProductDtoById(Guid id);
        Task CreateSizeSpecific(CreateSizeSpecificProductDto createSizeSpecificProduct);
        Task UpdateSizeSpecific(Guid id, UpdateSizeSpecificProductDto updateSizeSpecificProduct);
        Task<bool> ConfifProduct(ConfigProduct configProduct);
    }
    public class ConfigProductService : IConfigProductService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public ConfigProductService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<bool> ConfifProduct(ConfigProduct configProduct)
        {
            var boolSize = await ConfigSize(configProduct.ProId, configProduct.Size);

            var boolSubCat = await ConfigSubCat(configProduct.ProId, configProduct.SubCat);

            if (boolSize == true && boolSubCat == true) return true;

            return false;
        }

        public async Task CreateSizeSpecific(CreateSizeSpecificProductDto createSizeSpecificProduct)
        {
            createSizeSpecificProduct.Id = Guid.NewGuid();

            createSizeSpecificProduct.DateCreated = DateTime.Now;

            createSizeSpecificProduct.DateModified = DateTime.Now;

            SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProduct);

            _context.Add(sizeSpecificEachProduct);

            await _context.SaveChangesAsync();
        }

        public async Task<SizeSpecificProductDto> GetSizeSpecificProductDtoById(Guid id)
        {
            var item = await _context.SizeSpecificEachProduct.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {
                var sizeSpecificDto = _mapper.Map<SizeSpecificProductDto>(item);

                return sizeSpecificDto;
            }
            return null;
        }

        public async Task UpdateSizeSpecific(Guid id, UpdateSizeSpecificProductDto updateSizeSpecificProduct)
        {
            var item = await _context.SizeSpecificEachProduct.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {
                updateSizeSpecificProduct.DateCreated = item.DateCreated;
                updateSizeSpecificProduct.DateModified = DateTime.Now;

                _context.SizeSpecificEachProduct.Update(_mapper.Map(updateSizeSpecificProduct, item));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ConfigSize(Guid proId, List<int> sizeId)
        {
            using var transaction = _context.Database.BeginTransaction();

            var sizeSpecific = await _context.SizeSpecificEachProduct.Where(x => x.ProductId.Equals(proId)).ToListAsync();

            if (sizeId == null && sizeSpecific != null)
            {
                foreach (var item in sizeSpecific.Where(x => x.ActiveSize == true))
                {
                    item.ActiveSize = false;

                    item.DateModified = DateTime.Now;

                    UpdateSizeSpecificProductDto updateSizeSpecificProductDto = _mapper.Map<UpdateSizeSpecificProductDto>(item);

                    _context.Update(_mapper.Map(updateSizeSpecificProductDto, item));

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return true;
            }
            var sizeCheckedToList = sizeSpecific.Select(x => x.SizeId).Except(sizeId).ToList();

            if (sizeCheckedToList != null)
            {
                for (int i = 0; i < sizeCheckedToList.Count; i++)
                {
                    var item = sizeSpecific.FirstOrDefault(x => x.SizeId == sizeCheckedToList[i]);

                    item.ActiveSize = false;

                    item.DateModified = DateTime.Now;

                    UpdateSizeSpecificProductDto updateSizeSpecificProductDto = _mapper.Map<UpdateSizeSpecificProductDto>(item);

                    _context.Update(_mapper.Map(updateSizeSpecificProductDto, item));

                    await _context.SaveChangesAsync();
                }

            }
            var sizeCheckedToDb = sizeId.Except(sizeSpecific.Select(x => x.SizeId)).ToList();

            if (sizeCheckedToDb != null)
            {
                for (int i = 0; i < sizeId.Count; i++)
                {
                    if (!sizeSpecific.Any(x => x.SizeId == sizeId[i]))
                    {
                        CreateSizeSpecificProductDto createSizeSpecificProductDto = new()
                        {
                            Id = Guid.NewGuid(),

                            ProductId = proId,

                            SizeId = sizeId[i],

                            DateCreated = DateTime.Now,

                            DateModified = DateTime.Now,

                            ActiveNutri = false,

                            ActiveSize = true,
                        };

                        SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProductDto);

                        _context.Add(sizeSpecificEachProduct);

                        await _context.SaveChangesAsync();
                    }
                    if (sizeSpecific.Any(x => x.SizeId == sizeId[i] && x.ActiveSize == false))
                    {
                        var size = sizeSpecific.FirstOrDefault(x => x.SizeId == sizeId[i]);

                        size.DateModified = DateTime.Now;

                        size.ActiveSize = true;

                        UpdateSizeSpecificProductDto updateSizeSpecificProductDto = _mapper.Map<UpdateSizeSpecificProductDto>(size);

                        _context.Update(_mapper.Map(updateSizeSpecificProductDto, size));

                        await _context.SaveChangesAsync();
                    }
                }
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> ConfigSubCat(Guid proId, List<int> subCatId)
        {
            using var transaction = _context.Database.BeginTransaction();

            var subCatSpecific = await _context.SubCategorySpecificProducts.Where(x => x.ProductId.Equals(proId)).ToListAsync();

            if (subCatId == null && subCatSpecific != null)
            {
                foreach (var item in subCatSpecific.Where(x => x.Active == true))
                {
                    item.Active = false;

                    UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                    _context.Update(_mapper.Map(updateSubCategorySpecificProduct, item));

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return true;

            };


            var subCatCheckedToList = subCatSpecific.Select(x => x.SubCategoryId).Except(subCatId).ToList();

            if (subCatCheckedToList != null)
            {
                for (int i = 0; i < subCatCheckedToList.Count; i++)
                {
                    var item = subCatSpecific.FirstOrDefault(x => x.SubCategoryId == subCatCheckedToList[i]);

                    item.Active = false;

                    UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                    _context.Update(_mapper.Map(updateSubCategorySpecificProduct, item));

                    await _context.SaveChangesAsync();
                }

            }
            var subCatCheckedToDb = subCatId.Except(subCatSpecific.Select(x => x.SubCategoryId)).ToList();

            if (subCatCheckedToDb != null)
            {

                if (subCatCheckedToDb != null)
                {
                    for (int i = 0; i < subCatId.Count; i++)
                    {
                        if (!subCatSpecific.Any(x => x.SubCategoryId == subCatId[i]))
                        {
                            CreateSubCategorySpecificDto createSubCategorySpecific = new()
                            {
                                Id = Guid.NewGuid(),

                                ProductId = proId,

                                SubCategoryId = subCatId[i],

                                Active = true,
                            };

                            SubCategorySpecificProduct subCategorySpecificProduct = _mapper.Map<SubCategorySpecificProduct>(createSubCategorySpecific);

                            _context.Add(subCategorySpecificProduct);

                            await _context.SaveChangesAsync();
                        }
                        if (subCatSpecific.Any(x => x.SubCategoryId == subCatId[i] && x.Active == false))
                        {
                            var item = subCatSpecific.FirstOrDefault(x => x.SubCategoryId == subCatId[i]);

                            item.Active = true;

                            UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                            _context.Update(_mapper.Map(updateSubCategorySpecificProduct, item));

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            await transaction.CommitAsync();

            return true;
        }
    }
}
