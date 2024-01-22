using Microsoft.AspNetCore.Http;

namespace BJ.Contract.Category
{
    public class CreateCategoryDto
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        public string ImagePath { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
    }
}
