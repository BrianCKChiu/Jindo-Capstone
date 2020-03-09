using Jindo_Capstone.Model;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jindo_Capstone.Clients;

namespace Jindo_Capstone.Workers
{
    public class SendTextMessageJob
    {

        const string TxtMsgTempalte = "Hello {0}. This is an automated text message from CPOS. Would you like us to send a box of paper rolls? Type YES if you want to start the ordering process.";

        public void Execute()
        {
            SubscribedCustomer().ForEach(c =>
            {
                var twilioRestClient = new TwilioClient();
                String message = String.Format(TxtMsgTempalte, c.ContactName);
                using(DBContext db = new DBContext())
                {
                    Message msgObject = new Message()
                    {
                        customer = c,
                        MessageContent = message,
                        Date = DateTime.Now
                    };
                    System.Diagnostics.Debug.WriteLine("Message Sent");
                    //twilioRestClient.SendMessage(msgObject);
                    //db.Messages.Add(msgObject);
                }
            });
        }

        public static IEnumerable<Customer> SubscribedCustomer()
        {
            return new CustomerFinder().FindSubscribedCustomers();
        }
            
    }
}