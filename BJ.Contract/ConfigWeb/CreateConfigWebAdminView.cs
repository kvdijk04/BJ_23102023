using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.Translation.ConfigWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ConfigWeb
{
    public class CreateConfigWebAdminView
    {
        public CreateDetailConfigWebDto CreateDetailConfigWebDto { get; set; }

        public CreateDetailConfigWebTranslationDto CreateDetailConfigWebTranslationDto { get; set; }
        public bool NewPage { get; set; }

        public bool AutoSort { get;set; }
    }
}
