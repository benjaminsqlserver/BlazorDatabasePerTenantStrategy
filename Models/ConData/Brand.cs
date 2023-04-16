using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("brands", Schema = "production")]
    public partial class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int brand_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string brand_name { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}