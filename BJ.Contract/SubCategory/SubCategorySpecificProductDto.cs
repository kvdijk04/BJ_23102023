using BJ.Contract.Product;

namespace BJ.Contract.SubCategory
{
    public class SubCategorySpecificProductDto
    {
        public Guid Id { get; set; }
        public int SubCategoryId { get; set; }

        public Guid ProductId { get; set; }
        public bool Active { get; set; }
        public virtual ProductDto ProductDto { get; set; }
        public virtual SubCategoryDto SubCategoryDto { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

    }
}
