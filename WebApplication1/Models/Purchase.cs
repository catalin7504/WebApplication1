using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public partial class Purchase
    {
        [Display(Name ="Order ID")]
        public int Id { get; set; }
        public int UserId { get; set; }

        [Range(1,1000000)]
        public int ProductId { get; set; }

        [Range(1,10000)]
        public int Quantity { get; set; }

        [Display(Name ="Order Date")]
        [DisplayFormat(DataFormatString ="{0:MMM dd, yyyy}")]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Cancel Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? CancelDate { get; set; }

        [Display(Name = "Product")]
        public virtual Product Product { get; set; }
        public virtual Customer User { get; set; }
    }
}
