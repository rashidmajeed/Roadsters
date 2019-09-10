using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roadsters.Models
{
    public class Accessorie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Stock { get; set; }
        public enum EStock { NA = 0, InStock = 1, OutofStock = 2 }
        public string Image { get; set; }

    }
}
