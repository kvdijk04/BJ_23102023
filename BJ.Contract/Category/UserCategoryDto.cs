using BJ.Contract.Product;
using BJ.Contract.Translation.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Category
{
    public class UserCategoryDto
    {
        public Guid Id { get; set; }
        public string CatName { get; set; }
        public bool Active { get; set; }
        public string ImagePath { get; set; }
        public virtual List<UserProductDto> UserProductDtos { get; set; }
        public virtual List<CategoryTranslationDto> CategoryTranslationDtos { get; set; }

    }
}
