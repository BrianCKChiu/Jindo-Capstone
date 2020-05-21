using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jindo_Capstone.Models;

namespace Jindo_Capstone.Controllers
{
    public class MessageController : ApiController
    {

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
    }
}
