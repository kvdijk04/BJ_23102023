using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Domain.Entities
{
    public class StoreLocationTranslation
    {
        public Guid Id { get; set; }

        [ForeignKey("StoreLocation")]
        public int StoreLocationId { get; set; }

        [ForeignKey("Language")]
        public string LanguageId { get; set; }

        public string Name{ get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public Language Language { get; set; }

    }
}
