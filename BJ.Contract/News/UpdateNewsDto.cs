namespace BJ.Contract.News
{
    public class UpdateNewsDto
    {
        public string ImagePath { get; set; }

        public bool Active { get; set; }
        public bool Promotion { get; set; }

        public bool Popular { get; set; }

        public bool Home { get; set; }
        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateUpdated { get; set; }

        public string UserName { get; set; }
    }
}
