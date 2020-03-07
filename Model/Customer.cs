using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class Customer
    {
        [Required]
        public int CustID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime NextReminderTime { get; set; }
        public DateTime LastMessageDate { get; set; }
        
        [Required]
        public bool IsSubscribed { get; set; }
    }
}