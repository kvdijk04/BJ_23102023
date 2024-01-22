using Microsoft.AspNetCore.Http;

namespace BJ.Contract.SubCategory
{
    public class CreateSubCategoryDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
