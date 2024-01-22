using BJ.Contract.SubCategory;

namespace BJ.Contract.Translation.SubCategory
{
    public class SubCategoryTranslationDto
    {
        public Guid Id { get; set; }
        public string LanguageId { get; set; }

        public int SubCategoryId { get; set; }
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public SubCategoryDto SubCategoryDto { get; set; }
        public LanguageDto Language { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
}
