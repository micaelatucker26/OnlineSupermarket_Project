using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Models.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Item = new HashSet<Item>();
        }
        public int Id { get; set; }
        public ICollection<Item> Item { get; set; }
        public ICollection<Profile> Profile {get; set;}

        //public int profileId { get; set; }
        //public int itemId { get; set; }
    }
}
