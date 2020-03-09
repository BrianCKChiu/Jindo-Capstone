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
                //var customerQuery = FROM c IN db.Customers WHERE c.IsSubscribed AND c.MsgTypeId = MessageType SELECT c;
                //return customerQuery;
                return new Customer[] 
                { 
                    new Customer 
                    { 
                        CustID = 2,
                        ContactName = "Brian",
                        IsSubscribed = true,
                        PhoneNumber = "+16473278411",
                        MsgTypeId = 1
                    } 
                };
            }
            
        }

        //TODO: check if date is on the same day of message
        public IEnumerable<Customer> FindSubscribedCustomers(DateTime date)
        {
            using (DBContext db = new DBContext())
            {
                //var customerQuery = from c in db.Customers where c.IsSubscribed select c;
                //return customerQuery;
                return new Customer[]
                {
                    new Customer
                    {
                        CustID = 2,
                        ContactName = "Brian",
                        IsSubscribed = true,
                        PhoneNumber = "+16473278411"
                    }
                };
            }

        }

    }
}