using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("staffs", Schema = "sales")]
    public partial class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int staff_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string first_name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string last_name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string email { get; set; }

        [ConcurrencyCheck]
        public string phone { get; set; }

        [Required]
        [ConcurrencyCheck]
        public byte active { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int store_id { get; set; }

        [ConcurrencyCheck]
        public int? manager_id { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Staff Staff1 { get; set; }

        public ICollection<Staff> Staff2 { get; set; }

        public Store Store { get; set; }

    }
}