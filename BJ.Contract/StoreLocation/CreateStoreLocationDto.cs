using Microsoft.AspNetCore.Http;

namespace BJ.Contract.StoreLocation
{
    public class CreateStoreLocationDto
    {
        public int Id { get; set; }
        public string IconPath { get; set; }
        public string ImagePath { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Closed { get; set; }

        public bool Repaired { get; set; }
        public string Code { get; set; }
        public bool OpeningSoon { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateCreated { get; set; }

        public string UserName { get;set; }
    }
}
