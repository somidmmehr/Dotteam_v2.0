using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Dotteam.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Address { get; set; }
    }
}
