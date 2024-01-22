using System.ComponentModel.DataAnnotations.Schema;

namespace BJ.Domain.Entities
{
    public class Size
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        public string Note { get; set; }


        public int? Price { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }
        public bool Active { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<SizeSpecificEachProduct> SizeSpecificProducts { get; set; }
    }
}
