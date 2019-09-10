using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roadsters.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Bike> Bike { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }
        public IEnumerable<Accessorie> Accessorie { get; set; }

    }
}
