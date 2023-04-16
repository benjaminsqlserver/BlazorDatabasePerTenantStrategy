using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("stores", Schema = "sales")]
    public partial class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int store_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string store_name { get; set; }

        [ConcurrencyCheck]
        public string phone { get; set; }

        [ConcurrencyCheck]
        public string email { get; set; }

        [ConcurrencyCheck]
        public string street { get; set; }

        [ConcurrencyCheck]
        public string city { get; set; }

        [ConcurrencyCheck]
        public string state { get; set; }

        [ConcurrencyCheck]
        public string zip_code { get; set; }

        public ICollection<Stock> Stocks { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Staff> Staff { get; set; }

    }
}