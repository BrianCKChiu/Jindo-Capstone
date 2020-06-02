using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jindo_Capstone.Models;
using Jindo_Capstone.Clients;

namespace Jindo_Capstone.Controllers
{
    public class MessageController : ApiController
    {
        private const string TxtMsgTempalte = "Hello {0}. This is an automated text message from CPOS. Would you like us to send a box of paper rolls? Type YES if you want to start the ordering process.";

        public static void CreateReorderMessage(Customer customer)
        {
            String message = String.Format(TxtMsgTempalte, customer.ContactName);
            CreateMessage(customer, message);
        }

        //TODO: Change method name
        public static void CreateMessage(Customer customer, string message)
        {
            var twilioRestClient = new TwilioClient();
            using (DBContext db = new DBContext())
            {
                Message msgObject = new Message()
                {
                    Customer = customer,
                    CustID = customer.CustID,
                    MessageContent = message,
                    Date = DateTime.Now,
                    Msg = MessageType.Outbound
                };
                //System.Diagnostics.Debug.WriteLine(msgObject.Customer.ContactName);
                customer.LastMessaged = DateTime.Now;
                Customer cust = db.Customers.SingleOrDefault(c => customer.CustID == c.CustID);
                cust.LastMessaged = DateTime.Now;
                //SmsController.SendMessage(msgObject);
                db.Messages.Add(msgObject);
                db.SaveChanges();
            }
        }


        public static void AddIncomingMessage(Customer customer, string messageBody)
        {
            using (DBContext db = new DBContext())
            {
                Message msgObj = new Message()
                {
                    CustID = customer.CustID,
                    Date = DateTime.Now,
                    Msg = MessageType.Inbound,
                    MessageContent = messageBody
                };
                     db.Messages.Add(msgObj);
            }
        }

        public void AddMessage(Message msgObj)
        {
            using (DBContext db = new DBContext())
            {
                db.Messages.Add(msgObj);
            }
        }


    }
}
