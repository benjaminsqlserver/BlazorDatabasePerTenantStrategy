using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("orders", Schema = "sales")]
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int order_id { get; set; }

        [ConcurrencyCheck]
        public int? customer_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public byte order_status { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime order_date { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime required_date { get; set; }

        [ConcurrencyCheck]
        public DateTime? shipped_date { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int store_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int staff_id { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Customer Customer { get; set; }

        public Staff Staff { get; set; }

        public Store Store { get; set; }

    }
}