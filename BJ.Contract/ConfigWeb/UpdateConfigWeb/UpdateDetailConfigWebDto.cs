namespace BJ.Contract.ConfigWeb.UpdateConfigWeb
{
    public class UpdateDetailConfigWebDto
    {


        public DateTime? DateUpdated { get; set; }
        public string UserName { get;set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string Url { get; set; }
        public int SortOrder { get; set; }

        public bool NewPage { get;set; }

        public bool AutoSort { get; set; }

    }
}
