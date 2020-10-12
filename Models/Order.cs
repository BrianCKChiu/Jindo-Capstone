using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class Order
    {
        [DisplayName("Invoice Number")]
        [Required, Key]
        public int OrderID { get; set; }
        [Required]
        public int CustID { get; set; }
        public Customer Customer { 
            get
            {
                DBContext db = new DBContext();
                return (from c in db.Customers where CustID == c.CustID select c).SingleOrDefault();
            } 
        }
        [Required, DisplayName("Order Date")]
        public DateTime Date { get; set; }
        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int OrderAmount { get; set; }

        public String TrackingNumber { get; set; }
    }

    public enum OrderStatus
    {

        Shipped, NotShipped, Cancelled, Delayed 
    }
}