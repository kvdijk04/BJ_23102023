using BJ.Contract.Translation.SubCategory;

namespace BJ.Contract.SubCategory
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Code { get; set; }

        public virtual ICollection<SubCategorySpecificProductDto> SubCategorySpecificProductDtos { get; set; }
        public virtual ICollection<SubCategoryTranslationDto> SubCategoryTranslationDtos { get; set; }

    }
}
