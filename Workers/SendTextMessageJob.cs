using Jindo_Capstone.Model;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jindo_Capstone.Clients;

namespace Jindo_Capstone.Workers
{
    public static class SendTextMessageJob
    {

        const string TxtMsgTempalte = "Hello {0}. This is an automated text message from CPOS. Would you like us to send a box of paper rolls? Type YES if you want to start the ordering process.";

        public static void Execute()
        {
            SubscribedCustomer().ForEach(c => CreateMessage(c));
        }
        public static void Execute(int id)
        {
            SubscribedCustomer(id).ForEach(c => CreateMessage(c));
        }

        public static void CreateMessage(Customer customer)
        {
            var twilioRestClient = new TwilioClient();
            String message = String.Format(TxtMsgTempalte, customer.ContactName);
            using (DBContext db = new DBContext())
            {
                Message msgObject = new Message()
                {
                    Customer = customer,
                    CustID = customer.CustID,
                    MessageContent = message,
                    Date = DateTime.Now
                };
                //System.Diagnostics.Debug.WriteLine(msgObject.Customer.ContactName);
                customer.LastMessaged = DateTime.Now;
                Customer cust = db.Customers.SingleOrDefault(c => customer.CustID == c.CustID);
                cust.LastMessaged = DateTime.Now;
                //twilioRestClient.SendMessage(msgObject);
                db.Messages.Add(msgObject);
                db.SaveChanges();
            }
        }

        public static IEnumerable<Customer> SubscribedCustomer()
        {
            return new CustomerFinder().FindSubscribedCustomers();
        }
        public static IEnumerable<Customer> SubscribedCustomer(int id)
        {
            return new CustomerFinder().FindSubscribedCustomers(id);
        }

    }
}