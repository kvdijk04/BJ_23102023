using BJ.Contract.Translation.Category;
using BJ.Contract.Translation.Product;
using BJ.Contract.Translation.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation
{
    public class LanguageDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<CategoryTranslationDto> CategoryTranslationDtos { get; set; }
        public List<ProductTranslationDto> ProductTranslationDtos { get; set; }
        public List<SubCategoryTranslationDto> SubCategoryTranslationDtos { get; set; }

    }
}
