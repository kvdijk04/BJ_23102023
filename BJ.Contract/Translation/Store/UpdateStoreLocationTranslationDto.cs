using BJ.Contract.StoreLocation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.Store
{
    public class UpdateStoreLocationTranslationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? DateUpdated { get; set; }

        public string UserName { get;set; }
    }
}
