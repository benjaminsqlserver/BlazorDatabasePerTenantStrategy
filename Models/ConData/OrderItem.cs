using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("order_items", Schema = "sales")]
    public partial class OrderItem
    {
        [Key]
        [Required]
        public int order_id { get; set; }

        [Key]
        [Required]
        public int item_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int product_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int quantity { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal list_price { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal discount { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }

    }
}