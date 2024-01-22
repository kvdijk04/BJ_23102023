using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class StoreLocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IconPath { get; set; }
        public string ImagePath { get; set; }
        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Closed { get; set; }

        public bool Repaired { get; set; }

        public bool OpeningSoon {  get; set; }
        public int? Sort { get; set; }

        public string Code { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public List<StoreLocationOpenHour> StoreLocationOpenHours { get; set; }
        public List<StoreLocationTranslation> StoreLocationTranslations { get; set; }
    }
}
