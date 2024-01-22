namespace BJ.Contract.Blog
{
    public class CreateBlogDto
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }

        public bool Popular { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public string UserName { get;set; }

        public string Code { get; set; }

    }
}
