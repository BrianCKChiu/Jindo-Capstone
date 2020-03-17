using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class Customer
    {
        [NotMapped]
        public bool IsChecked { get; set; }
        [Required, Key]
        public int CustID { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }

        [Required]
        public bool IsSubscribed { get; set; }
        public DateTime? LastMessaged { get; set; }
    }
}