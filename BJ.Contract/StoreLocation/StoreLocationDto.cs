using BJ.Contract.Translation.Store;

namespace BJ.Contract.StoreLocation
{
    public class StoreLocationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string IconPath { get; set; }
        public string ImagePath { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Closed { get; set; }

        public bool Repaired { get; set; }
        public bool OpeningSoon { get; set; }
        public int? Sort { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public List<StoreLocationOpenHourDto> StoreLocationOpenHourDtos { get; set; }
        public List<StoreLocationTranslationDto> StoreLocationTranslationDtos { get; set; }
    }
}
