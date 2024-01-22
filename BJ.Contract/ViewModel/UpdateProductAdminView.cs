using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Product;
using Microsoft.AspNetCore.Http;

namespace BJ.Contract.ViewModel
{
    public class UpdateProductAdminView
    {
        public UpdateProductDto UpdateProductDto { get; set; }
        public UpdateProductTranslationDto UpdateProductTranslationDto { get; set; }
        public Guid CategoryId { get; set; }
        public List<SizeSpecificProductDto> SizeSpecificProductDto { get; set; }
        public List<SubCategorySpecificProductDto> SubCategorySpecificProductDtos { get; set; }
        public List<int> Size { get; set; }
        public IFormFile ImageCup { get; set; }
        public IFormFile ImageHero { get; set; }

        public IFormFile ImageIngredients { get; set; }
    }
}
