using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class BlogTranslation
    {
        public Guid Id { get; set; }

        [ForeignKey("Blog")]
        public Guid BlogId { get; set; }

        [ForeignKey("Language")]
        public string LanguageId { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }

        public Language Language { get; set; }

        public Blog Blog { get; set; }
    }
}
