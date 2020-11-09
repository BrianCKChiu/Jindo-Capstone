using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jindo_Capstone.Models;
using Jindo_Capstone.Clients;
using System.Web.Configuration;

namespace Jindo_Capstone.Controllers
{
    public class MessageController : ApiController
    {

        private readonly static string[] validText = { "yes", "no", "terminate" };
        /// <summary>
        /// Creates a re-order message to a customer
        /// </summary>
        /// <param name="customer">the recipient that the message is sent to</param>
        public static void CreateReorderMessage(Customer customer)
        {
            String message = String.Format(WebConfigurationManager.AppSettings["ReorderMsg"], customer.ContactName);
            CreateOutgoingMessage(customer, message, MessageType.Request);
        }

        /// <summary>
        /// Creates an outgoing text message
        /// </summary>
        /// <param name="customer">Recipient that the message is sent</param>
        /// <param name="message">Message body</param>
        /// <param name="msgType">The type of message </param>
        public static void CreateOutgoingMessage(Customer customer, string message, MessageType msgType)
        {
            SmsController Messenger = new SmsController();

            //logs the message to the database
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

                //updates last sent message to the recipient
                UpdateLastMesseged(customer);

                if (msgType == MessageType.Request)
                {
                    String msgSID = Messenger.SendMessage(msgObject); 
                    msgObject.MessageSID = msgSID;
                }

                SmsController smsController = new SmsController();
                smsController.SendMessage(msgObject);
                db.Messages.Add(msgObject);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Updates last received/sent message of a recipient
        /// </summary>
        /// <param name="cust">recipient that set/recieved a message</param>
        private static void UpdateLastMesseged(Customer cust)
        {
            using (DBContext db = new DBContext())
            {
                Customer customer = db.Customers.SingleOrDefault(c => cust.CustID == c.CustID);
                customer.LastMessaged = DateTime.Now;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Saves an incoming message received by the Twilio number
        /// </summary>
        /// <param name="customer">The customer who sent the message</param>
        /// <param name="messageBody">the message in the text message</param>
        /// <param name="msgSID">Message ID</param>
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
        /// <summary>
        /// Determines what type of incoming message received by Twilio 
        /// </summary>
        /// <param name="message">Message the user has sent</param>
        /// <param name="customerID">The customer who've sent the message</param>
        /// <returns>Type of message</returns>
        public static MessageType DetermineMessageType(String message, int customerID)
        {

            DBContext db = new DBContext();
            var formatedMsg = FormatMsg(message);

            if (IsTextValid(formatedMsg))
            {
                if (message.Equals(WebConfigurationManager.AppSettings["UnsubscribeString"]))
                {
                    return MessageType.Unsubscribe;
                }

                return DetermineResponseType(formatedMsg, customerID);
            }
            return MessageType.Error;
        }


        //change method name
        private static MessageType DetermineResponseType(String message, int customerID) {
            using (DBContext db = new DBContext())
            {
                var latestMsg = (from m in db.Messages where m.CustID == customerID orderby m.Date descending select m).FirstOrDefault();
                
            
                if (latestMsg != null)
                {
                    if (latestMsg.Type == MessageType.Request || latestMsg.Type == MessageType.Invalid || latestMsg.Type == MessageType.Inbound)
                    {

                        //chhange this to a switch statement 
                        if (message.Equals(WebConfigurationManager.AppSettings["ConfirmString"]))
                        {
                            return MessageType.Confirmation;
                        }
                        else
                        {
                            return MessageType.Decline;
                        }
                    }
                }

            return MessageType.Invalid;

            }
        }
        /// <summary>
        /// Checks if customer's reponse is a valid response to order a new set of paper roll
        /// </summary>
        /// <param name="message">Customer's response message</param>
        /// <returns></returns>
        private static bool IsTextValid(string message)
        {
            return validText.Contains(message);
        }
        /// <summary>
        /// Formates message to all lowercase
        /// </summary>
        /// <param name="message">String that needs to be formated</param>
        /// <returns>Formated string</returns>
        private static string FormatMsg(string message)
        {
            return message.Trim().ToLower();
        }

    }
}
