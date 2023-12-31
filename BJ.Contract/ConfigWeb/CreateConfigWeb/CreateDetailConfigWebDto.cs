﻿namespace BJ.Contract.ConfigWeb.CreateConfigWeb
{
    public class CreateDetailConfigWebDto
    {
        public Guid Id { get; set; }

        public int ConfigWebId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool Active { get; set; }
        public int SortOrder { get; set; }

    }
}
