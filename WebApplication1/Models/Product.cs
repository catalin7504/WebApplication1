using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public partial class Product
    {
        public Product()
        {
            Purchase = new HashSet<Purchase>();
        }

        [Display(Name="Product")]
        public int Id { get; set; }

        [Range(1,100)]
        [Required]
        public string Name { get; set; }

        [Range(1, 1000)]
        public string Description { get; set; }

        [Display(Name="Price")]
        [DisplayFormat(DataFormatString ="{0:C2}")]
        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
