using Jindo_Capstone.Model;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Jindo_Capstone.Workers
{
    public class SendTextMessageJob
    {

        const string MessageTempalte = "Hello {} text me plox";

        public void Execute()
        {
            SubscribedCustomer().ForEach(c =>
            {

                Console.WriteLine(c.Name);
            }
                );
        }

        public static IEnumerable<Customer> SubscribedCustomer()
        {
            return new CustomerFinder().FindSubscribedCustomers();
        }
            
    }
}