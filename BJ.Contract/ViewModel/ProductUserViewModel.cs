using BJ.Contract.Category;
using BJ.Contract.Product;

namespace BJ.Contract.ViewModel
{
    public class ProductUserViewModel
    {
        public List<UserCategoryDto> UserCategoryDtos { get; set; }
        public List<UserProductDto> UserProductDtos { get; set; }
    }
}
