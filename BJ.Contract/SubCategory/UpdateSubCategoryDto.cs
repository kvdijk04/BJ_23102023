using Microsoft.AspNetCore.Http;

namespace BJ.Contract.SubCategory
{
    public class UpdateSubCategoryDto
    {
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public IFormFile Image { get; set; }

    }
}
