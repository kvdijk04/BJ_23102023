using BJ.Contract.Category;

namespace BJ.Contract.Translation.Category
{
    public class CategoryTranslationDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string LanguageId { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public CategoryDto Category { get; set; }
    }
}
