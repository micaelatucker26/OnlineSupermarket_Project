using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Type { get; set; }
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Range(0, 1000)]
        public int AmountInStock { get; set; }
        public int QuantityToBuy { get; set; }
        //public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
