using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public int? Discount { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeTag { get; set; }
        public bool Active { get; set; }
        public string Tags { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }

        public string ImagePathCup { get; set; }

        public string ImagePathHero { get; set; }

        public string ImagePathIngredients { get; set; }

        public string MetaKey { get; set; }
        public string Code { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<SizeSpecificEachProduct> SizeSpecificProducts { get; set; }
        public virtual ICollection<SubCategorySpecificProduct> SubCategorySpecificProducts { get; set; }

    }
}
