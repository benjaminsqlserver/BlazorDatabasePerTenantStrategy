using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models.ConData
{
    [Table("categories", Schema = "production")]
    public partial class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int category_id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string category_name { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}