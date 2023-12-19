namespace BJ.Domain.Entities
{
    public class DetailConfigWebsite
    {
        public Guid Id { get; set; }

        public int ConfigWebId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public ConfigWebsite ConfigWeb { get; set; }
        
        public bool Active { get;set; }

        public int SortOrder { get; set; }
        public List<DetailConfigWebsiteTranslation> DetailConfigWebTranslations { get; set; }
    }
}
