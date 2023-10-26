using BJ.Contract.Category;
using BJ.Contract.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class ProductUserViewModel
    {
        public List<UserCategoryDto> UserCategoryDtos{ get; set; }
        public List<UserProductDto> UserProductDtos { get; set;}
    }
}
