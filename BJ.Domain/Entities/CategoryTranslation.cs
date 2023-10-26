using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Domain.Entities
{
    public class CategoryTranslation
    {
        public Guid Id { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        [ForeignKey("Language")]
        public string LanguageId { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public Category Category { get; set; }
        public Language Language { get; set; }

    }
}
