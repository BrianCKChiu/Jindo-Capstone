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
        //private static SmsController Messenger = new SmsController();

            //RENAME
        public static void CreateReorderMessage(Customer customer)
        {
            String message = String.Format(TxtMsgTempalte, customer.ContactName);
            CreateOutgoingMessage(customer, message, MessageType.Request);
        }

        //TODO: Change method name
        public static void CreateOutgoingMessage(Customer customer, string message, MessageType msgType)
        {
            SmsController Messenger = new SmsController();
            var twilioRestClient = new TwilioClient();
            using (DBContext db = new DBContext())
            {
                Message msgObject = new Message()
                    {
                        Customer = customer,
                        CustID = customer.CustID,
                        MessageContent = message,
                        Date = DateTime.Now,
                        Type = msgType,
                        MessageSID = null
                    };

                //updates last sent message
                customer.LastMessaged = DateTime.Now;
                Customer cust = db.Customers.SingleOrDefault(c => customer.CustID == c.CustID);
                cust.LastMessaged = DateTime.Now;
                //
                if (msgType == MessageType.Request)
                {
                    String msgSID = Messenger.SendMessage(msgObject); //commentted out for now BUT works
                    msgObject.MessageSID = msgSID;
                }
                db.Messages.Add(msgObject);
                db.SaveChanges();
            }
        }


        public static void AddIncomingMessage(Customer customer, string messageBody, string msgSID)
        {
            using (DBContext db = new DBContext())
            {
                Message msgObj = new Message()
                {
                    CustID = customer.CustID,
                    Date = DateTime.Now,
                    Type = MessageType.Inbound,
                    MessageContent = messageBody,
                    MessageSID = msgSID
                };
                db.Messages.Add(msgObj);
                db.SaveChanges();
            }
        }

        public void AddMessage(Message msgObj)
        {
            using (DBContext db = new DBContext())
            {
                db.Messages.Add(msgObj);
                db.SaveChanges();
            }
        }


    }
}
