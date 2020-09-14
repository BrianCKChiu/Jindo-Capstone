using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Razor.Generator;

namespace Jindo_Capstone.Controllers
{
    public class OrderController : ApiController
    {
        public static Order CreateOrder(Customer customer)
        {
            DBContext db = new DBContext();
            Order order = new Order
            {
               InvoiceNumber = GenerateInvoiceNumber(1),
               CustomerID = customer.CustID,
               Date = DateTime.Now,
               OrderAmount = 10,
               Status = OrderStatus.NotShipped
            };

            db.Orders.Add(order);
            return order;
        }

        private static int GenerateInvoiceNumber(int id)
        {
            //(id : number + string)

            return 0;
        }
    }
}
