using BJ.Contract.Product;
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
        public virtual ICollection<UserProductDto> UserProductDtos { get; set; } = new HashSet<UserProductDto>();
    }
}
