using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Models.Entities
{
    public class Profile
    {
        public string Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        [StringLength(2)]
        public string State { get; set; }
        [Required]
        [StringLength(5)]
        public string Zip { get; set; }
        //
        //public ApplicationUser ApplicationUser { get; set; }
    }
}
