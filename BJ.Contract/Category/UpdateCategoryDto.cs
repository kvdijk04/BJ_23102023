using Microsoft.AspNetCore.Http;

namespace BJ.Contract.Category
{
    public class UpdateCategoryDto
    {
        //public int? ParentId { get; set; }
        //public int? Levels { get; set; }
        //public int? Ordering { get; set; }
        public bool Active { get; set; }
        public string ImagePath { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserName { get; set; }

    }
}
