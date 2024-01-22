using BJ.Contract.Size;
using BJ.Contract.SubCategory;

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
        public string Alias { get; set; }
        public string ImagePathCup { get; set; }
        public string ImagePathHero { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public string ImagePathIngredients { get; set; }
        public virtual List<SizeSpecificProductDto> SizeSpecificProducts { get; set; }
        public virtual List<UserSubCategorySpecificProductDto> UserSubCategorySpecificProductDto { get; set; }

    }

}
