using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class CategoryTranslation
    {
        public Guid Id { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        [ForeignKey("Language")]
        public string LanguageId { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public Category Category { get; set; }
        public Language Language { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
}
