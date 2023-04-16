using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("customers", Schema = "sales")]
    public partial class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customer_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string first_name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string last_name { get; set; }

        [ConcurrencyCheck]
        public string phone { get; set; }

        [Required]
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

        public ICollection<Order> Orders { get; set; }

    }
}