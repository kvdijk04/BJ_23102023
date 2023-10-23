using BJ.Contract.Category;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Product
{
    public class UserProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string CatName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeTag { get; set; }
        public bool Active { get; set; }
        public string ImagePathCup { get; set; }

        public string ImagePathHero { get; set; }

        public string ImagePathIngredients { get; set; }
        public virtual ICollection<SizeSpecificProductDto> SizeSpecificProducts { get; set; }
        public virtual ICollection<UserSubCategorySpecificProductDto> UserSubCategorySpecificProductDto { get; set; }
    }

}
