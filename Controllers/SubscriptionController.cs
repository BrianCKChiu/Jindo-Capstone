using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jindo_Capstone.Controllers
{
    public class SubscriptionController : ApiController
    {
        // GET: api/Subscription

        public static int Unsubscribe(int id)
        {
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Unsubscribe the customer
                customer.IsSubscribed = false;
                 return db.SaveChanges();
            }
        }

        public static int Subscribe(int id)
        {
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                customer.IsSubscribed = true;
                return db.SaveChanges();
            }
        }


    }
}
