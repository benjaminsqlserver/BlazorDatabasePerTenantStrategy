using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("stocks", Schema = "production")]
    public partial class Stock
    {
        [Key]
        [Required]
        public int store_id { get; set; }

        [Key]
        [Required]
        public int product_id { get; set; }

        [ConcurrencyCheck]
        public int? quantity { get; set; }

        public Product Product { get; set; }

        public Store Store { get; set; }

    }
}