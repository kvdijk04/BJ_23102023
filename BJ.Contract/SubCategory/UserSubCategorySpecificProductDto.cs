using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.SubCategory
{
    public class UserSubCategorySpecificProductDto
    {
        public Guid Id { get; set; }
        public int SubCategoryId { get; set; }

        public Guid ProductId { get; set; }
        public bool ActiveProduct { get; set; }
        public string SubCatName { get; set; }
        public string ImagePath { get; set; }
        public bool SubActive { get; set; }
        public SubCategoryDto SubCategoryDto { get; set; }
    }
}
