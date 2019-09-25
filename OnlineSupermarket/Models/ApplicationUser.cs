using Microsoft.AspNetCore.Identity;
using OnlineSupermarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Models
{
    public class ApplicationUser: IdentityUser
    {
        public override string Id { get; set; }
        public IdentityUser User { get; set; }

        public ICollection<IdentityRole> Roles { get; set; }

       // public Profile Profile { get; set; }

        public ApplicationUser()
        {
            Roles = new List<IdentityRole>();
        }

        public bool HasRole(string roleName)
        {
            return Roles.FirstOrDefault(r => r.Name == roleName) != null;
        }
    }
}
