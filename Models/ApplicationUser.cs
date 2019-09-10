using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roadsters.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}
