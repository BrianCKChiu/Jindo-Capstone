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
        public IEnumerable<Customer> FindSubscribedCustomers()
        {
            using(DBContext db = new DBContext())
            {
                //var customerQuery = from c in db.Customers where c.IsSubscribed select c;
                //return customerQuery;
                return new Customer[] 
                { 
                    new Customer 
                    { 
                        CustID = 2,
                        Name = "Brian",
                        IsSubscribed = true,
                        PhoneNumber = "+16473278411"
                    } 
                };
            }
            
        }
    }
}