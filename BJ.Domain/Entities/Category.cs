namespace BJ.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public int? Sort { get;set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual List<Size> Sizes { get; set; }
    }
}
