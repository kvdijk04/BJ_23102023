namespace BJ.Domain.Entities
{
    public class Language
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }
        public List<SubCategoryTranslation> SubCategoryTranslations { get; set; }
        public List<NewsTranslation> NewsTranslations { get; set; }
        //public List<DetailConfigWebTranslation> DetailConfigWebTranslations { get; set; }
        public List<BlogTranslation> BlogTranslations { get; set; }
        public List<StoreLocationTranslation> StoreLocationTranslations{ get; set; }

    }
}
