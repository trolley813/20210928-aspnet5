using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace _20210928.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the StoreUser class
    public class StoreUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}
