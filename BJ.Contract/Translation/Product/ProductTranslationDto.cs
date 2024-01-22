using BJ.Contract.Product;

namespace BJ.Contract.Translation.Product
{
    public class ProductTranslationDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string LanguageId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }

        public ProductDto ProductDto { get; set; }
        public LanguageDto Language { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
