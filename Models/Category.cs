using System.ComponentModel.DataAnnotations;

namespace Roadsters.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name="Category Name")]

        [Required]
        public string Name { get; set; }
    }
}
