using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("products", Schema = "production")]
    public partial class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string product_name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int brand_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int category_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public short model_year { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal list_price { get; set; }

        public Brand Brand { get; set; }

        public Category Category { get; set; }

        public ICollection<Stock> Stocks { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}