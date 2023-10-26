using BJ.Contract.Category;
using BJ.Contract.SubCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.SubCategory
{
    public class UpdateSubCategoryTranslationDto
    {
        public string SubCatName { get; set; }
        public string Description { get; set; }
    }
}
