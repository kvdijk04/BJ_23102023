using BJ.Contract.Product;
using BJ.Contract.Translation.Category;

namespace BJ.Contract.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }

        public bool Active { get; set; }
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<ProductDto> ProductDtos { get; set; } = new HashSet<ProductDto>();
        public virtual List<CategoryTranslationDto> CategoryTranslationDtos { get; set; }

    }
}
