using Microsoft.AspNetCore.Http;

namespace BJ.Contract.Category
{
    public class UpdateCategoryDto
    {
        public string CatName { get; set; }
        public string Description { get; set; }
        //public int? ParentId { get; set; }
        //public int? Levels { get; set; }
        //public int? Ordering { get; set; }
        public bool Active { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateUpdated { get; set; }
        public IFormFile Image { get; set; }

    }
}
