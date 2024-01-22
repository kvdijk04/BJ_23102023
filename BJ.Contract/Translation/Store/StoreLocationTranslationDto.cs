using BJ.Contract.StoreLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.Store
{
    public class StoreLocationTranslationDto
    {
        public Guid Id { get; set; }

        public int StoreLocationId { get; set; }
        public string LanguageId { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public StoreLocationDto StoreLocationDto { get; set; }
        public LanguageDto Language { get; set; }
    }
}
