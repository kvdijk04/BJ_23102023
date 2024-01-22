namespace BJ.Contract.Translation.SubCategory
{
    public class CreateSubCategoryTranslationDto
    {
        public Guid Id { get; set; }
        public int SubCategoryId { get; set; }
        public string LanguageId { get; set; }
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }

        public string UserName { get; set; }
    }
}
