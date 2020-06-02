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
        public int InvoiceNumber { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public Customer Customer { 
            get
            {
                DBContext db = new DBContext();
                return (from c in db.Customers where CustomerID == c.CustID select c).Single();
            } 
        }
        [Required, DisplayName("Order Date")]
        public DateTime Date { get; set; }
        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int OrderAmount { get; set; }
    }

    public enum OrderStatus
    {
        //WIP
        Shipped, NotShipped
    }
}