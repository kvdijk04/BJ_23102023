using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class SubCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<SubCategorySpecificProduct> SubCategorySpecificProducts { get; set; }
        public virtual ICollection<SubCategoryTranslation> SubCategoryTranslations { get; set; }

    }
}
