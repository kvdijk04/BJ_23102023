using BJ.Contract.Translation.ConfigWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class ConfigWebViewModel
    {
        public Guid Id { get; set; }

        public int ConfigId { get; set; }

        public string NameConfig { get; set; }

        public DateTime DateCreated { get;set; }

        public DateTime DateUpdated { get;set; }

        public string LanguageId { get;set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Active { get;set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }

        public List<DetailConfigWebTranslationDto> DetailConfigWebTranslationDtos { get; set; }
    }
}
