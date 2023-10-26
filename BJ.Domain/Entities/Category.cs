namespace BJ.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<CategoryTranslation> CategoryTranslations { get; set; }

    }
}
