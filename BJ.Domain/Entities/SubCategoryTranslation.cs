using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class SubCategoryTranslation
    {
        public Guid Id { get; set; }
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("Language")]
        public string LanguageId { get; set; }
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public SubCategory SubCategory { get; set; }
        public Language Language { get; set; }

    }
}
