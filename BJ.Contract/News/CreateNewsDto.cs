namespace BJ.Contract.News
{
    public class CreateNewsDto
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }

        public bool Active { get; set; }

        public bool Popular { get; set; }
        public bool Promotion { get; set; }

        public bool Home { get; set; }
        public string Code { get; set; }

        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }

        public string UserName { get; set; }
    }
}
