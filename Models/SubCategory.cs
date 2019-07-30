using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roadsters.Models
{
    public class SubCategory 
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "SubCategory  Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}