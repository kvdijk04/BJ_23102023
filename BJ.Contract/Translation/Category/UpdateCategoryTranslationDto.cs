namespace BJ.Contract.Translation.Category
{
    public class UpdateCategoryTranslationDto
    {
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserName { get; set; }
    }
}
