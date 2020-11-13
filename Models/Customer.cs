using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class Customer
    {
        [NotMapped]
        public bool IsChecked { get; set; }
        [Required, Key]
        public int CustID { get; set; }
        [Required]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        [Required]
        public bool IsSubscribed { get; set; }
        [DisplayFormat(NullDisplayText = "Never")]
        public DateTime? LastMessaged { get; set; }
    }

    // Inclusion of CustomerList as a result of the customer batch submit form requiring this object type
    public class CustomerList { 
        public List<Customer> customers { get; set; }
    }
}