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

        const string TxtMsgTempalte = "Hello {1}. This is an automated text message from CPOS. Would you like us to send a box of paper rolls? Type YES if you want to start the ordering process.";

        public void Execute()
        {
            SubscribedCustomer().ForEach(c =>
            {
                String message = String.Format(TxtMsgTempalte, c.Name);
                using(DBContext db = new DBContext())
                {
                    Message msgObject = new Message()
                    {
                        Customer = c,
                        MessageContent = message
                    };
                    
                    db.Messages.Add(msgObject);
                }
                Console.WriteLine(c.Name);
            });
        }

        public static IEnumerable<Customer> SubscribedCustomer()
        {
            return new CustomerFinder().FindSubscribedCustomers();
        }
            
    }
}