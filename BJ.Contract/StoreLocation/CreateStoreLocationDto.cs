using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.StoreLocation
{
    public class CreateStoreLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string IconPath { get; set; }
        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Closed { get; set; }

        public bool Repaired { get; set; }
    }
}
