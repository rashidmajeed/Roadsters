using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roadsters.Models.ViewModels
{
    public class CategoryAndSubCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<string> SubCategoryList { get; set; }
        public string AlertMessage { get; set; }
    }
}
