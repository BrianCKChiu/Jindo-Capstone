using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jindo_Capstone.Model;

namespace Jindo_Capstone
{
    public class CustomerFinder
    {

        
        //for texting
        public IEnumerable<Customer> FindSubscribedCustomers(/*int MessageType  <-- Once Email function is added*/)
        {
            using(DBContext db = new DBContext())
            {
                var customerQuery = from c in db.Customers where c.IsSubscribed select c;
                return customerQuery.ToList();

            }
            
        }

        //TODO: check if date is on the same day of message
        public IEnumerable<Customer> FindSubscribedCustomers(int id)
        {
            using (DBContext db = new DBContext())
            {
                var customerQuery = from c in db.Customers where c.IsSubscribed && c.CustID == id select c;
                return customerQuery.ToList();
            }

        }

    }
}