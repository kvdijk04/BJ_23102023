using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.StoreLocation
{
    public class CreateStoreLocationOpenHourDto
    {
        public int StoreLocationId { get; set; }

        public string DaysOfWeek { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool Active { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UserName { get; set; }    
    }
}
