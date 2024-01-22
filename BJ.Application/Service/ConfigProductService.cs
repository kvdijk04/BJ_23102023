using AutoMapper;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Drawing;

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
            var boolSize = await ConfigSize(configProduct.ProId,configProduct.CategoryId, configProduct.NewSize, configProduct.Size, configProduct.UserName);

            var boolSubCat = await ConfigSubCat(configProduct.ProId, configProduct.SubCat,configProduct.UserName);

            if (boolSize == true && boolSubCat == true) return true;

            return false;
        }

        public async Task CreateSizeSpecific(CreateSizeSpecificProductDto createSizeSpecificProduct)
        {
            createSizeSpecificProduct.Id = Guid.NewGuid();

            createSizeSpecificProduct.DateCreated = DateTime.Now;


            SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProduct);

            _context.Add(sizeSpecificEachProduct);

            await _context.SaveChangesAsync(createSizeSpecificProduct.UserName);
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
                updateSizeSpecificProduct.DateModified = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateSizeSpecificProduct);

                await _context.SaveChangesAsync(updateSizeSpecificProduct.UserName);
            }
        }

        public async Task<bool> ConfigSize(Guid proId,Guid categoryId, List<int> newSizeId, List<int> sizeId,string userName)
        {
            //if(updateProductAdminView.UpdateProductDto.CategoryId.Equals(item.CategoryId) == false)
            //{
            //    var oldSize = _context.SizeSpecificEachProduct.Where(x => x.ProductId.Equals(id));
            //    _context.RemoveRange(oldSize);
            //    await _context.SaveChangesAsync(updateProductAdminView.UpdateProductDto.UserName);
            //}
            var checkCatId = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(proId));



            using var transaction = _context.Database.BeginTransaction();

            if (categoryId.Equals(checkCatId.CategoryId) == false)
            {
                _context.Entry(checkCatId).Property(x => x.CategoryId).CurrentValue = categoryId;



                await _context.SaveChangesAsync(userName);

                var oldSize = _context.SizeSpecificEachProduct.Where(x => x.ProductId.Equals(proId));

                _context.RemoveRange(oldSize);

                await _context.SaveChangesAsync(userName);

                await transaction.CommitAsync();

                sizeId = newSizeId;
            }

            var sizeSpecific = await _context.SizeSpecificEachProduct.Where(x => x.ProductId.Equals(proId)).ToListAsync();

            if (sizeId == null && sizeSpecific != null)
            {
                foreach (var item in sizeSpecific.Where(x => x.ActiveSize == true))
                {
                    item.ActiveSize = false;

                    item.DateModified = DateTime.Now;

                    UpdateSizeSpecificProductDto updateSizeSpecificProductDto = _mapper.Map<UpdateSizeSpecificProductDto>(item);

                    _context.Entry(item).CurrentValues.SetValues(updateSizeSpecificProductDto);

                    await _context.SaveChangesAsync(userName);
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

                    _context.Entry(item).CurrentValues.SetValues(updateSizeSpecificProductDto);

                    await _context.SaveChangesAsync(userName);
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

                            ActiveNutri = false,

                            ActiveSize = true,
                        };

                        SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProductDto);

                        _context.Add(sizeSpecificEachProduct);

                        await _context.SaveChangesAsync(userName);
                    }
                    if (sizeSpecific.Any(x => x.SizeId == sizeId[i] && x.ActiveSize == false))
                    {
                        var size = sizeSpecific.FirstOrDefault(x => x.SizeId == sizeId[i]);

                        size.DateModified = DateTime.Now;

                        size.ActiveSize = true;

                        UpdateSizeSpecificProductDto updateSizeSpecificProductDto = _mapper.Map<UpdateSizeSpecificProductDto>(size);

                        _context.Entry(size).CurrentValues.SetValues(updateSizeSpecificProductDto);

                        await _context.SaveChangesAsync(userName);
                    }
                }
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> ConfigSubCat(Guid proId, List<int> subCatId, string userName)
        {
            using var transaction = _context.Database.BeginTransaction();

            var subCatSpecific = await _context.SubCategorySpecificProducts.Where(x => x.ProductId.Equals(proId)).ToListAsync();

            if (subCatId == null && subCatSpecific != null)
            {
                foreach (var item in subCatSpecific.Where(x => x.Active == true))
                {
                    item.Active = false;

                    item.DateUpdated = DateTime.Now;

                    UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                    _context.Entry(item).CurrentValues.SetValues(updateSubCategorySpecificProduct);

                    await _context.SaveChangesAsync(userName);
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

                    item.DateUpdated = DateTime.Now;

                    UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                    _context.Entry(item).CurrentValues.SetValues(updateSubCategorySpecificProduct);

                    await _context.SaveChangesAsync(userName);
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

                                DateCreated = DateTime.Now,
                            };

                            SubCategorySpecificProduct subCategorySpecificProduct = _mapper.Map<SubCategorySpecificProduct>(createSubCategorySpecific);

                            _context.Add(subCategorySpecificProduct);

                            await _context.SaveChangesAsync(userName);
                        }
                        if (subCatSpecific.Any(x => x.SubCategoryId == subCatId[i] && x.Active == false))
                        {
                            var item = subCatSpecific.FirstOrDefault(x => x.SubCategoryId == subCatId[i]);

                            item.Active = true;

                            item.DateUpdated = DateTime.Now;

                            UpdateSubCategorySpecificProduct updateSubCategorySpecificProduct = _mapper.Map<UpdateSubCategorySpecificProduct>(item);

                            _context.Entry(item).CurrentValues.SetValues(updateSubCategorySpecificProduct);

                            await _context.SaveChangesAsync(userName);
                        }
                    }
                }
            }

            await transaction.CommitAsync();

            return true;
        }
    }
}
