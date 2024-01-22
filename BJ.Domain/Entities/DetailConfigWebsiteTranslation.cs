using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class DetailConfigWebsiteTranslation
    {
        public Guid Id { get; set; }

        [ForeignKey("DetailConfigWeb")]
        public Guid DetailConfigWebId { get; set; }

        [ForeignKey("Language")]
        public string LanguageId { get; set; }

        public string Title { get;set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public DetailConfigWebsite DetailConfigWeb { get; set; }

        public Language Language { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
