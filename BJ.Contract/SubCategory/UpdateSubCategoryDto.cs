using Microsoft.AspNetCore.Http;

namespace BJ.Contract.SubCategory
{
    public class UpdateSubCategoryDto
    {
        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public IFormFile Image { get; set; }
        public string UserName { get; set; }

    }
}
