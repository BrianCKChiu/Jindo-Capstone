using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.AspNet.Common;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using Jindo_Capstone.Models;
using System.Web.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Jindo_Capstone.Controllers
{
    public class SmsController : TwilioController
    {
        private readonly ITwilioRestClient _client;
        private readonly string _tokenAuth = WebConfigurationManager.AppSettings["AuthToken"];
        private readonly string _accountSid = WebConfigurationManager.AppSettings["AccountSid"];
        private readonly string _twilioNumber = WebConfigurationManager.AppSettings["TwilioNumber"];

        public SmsController()
        {
            _client = new TwilioRestClient(_accountSid, _tokenAuth);
        }
        public SmsController(TwilioRestClient client)
        {
            _client = client;
        }
        /// <summary>
        /// Sends a outgoing message 
        /// </summary>
        /// <param name="msg">message that needs to be sent</param>
        /// <returns>Message SID that Twilio has created for the message</returns>
        public string SendMessage(Message msg)
        {
            //sends message
            var msgObject = MessageResource.Create(
                body: msg.MessageContent,
                from: _twilioNumber,
                to: msg.Customer.PhoneNumber,
                client: _client);
            //returns unique ID of msg
            return msgObject.Sid;
        }


        //[Sms/Incoming]
        /// <summary>
        /// Process incomming messages received 
        /// </summary>
        /// <param name="incomingMessage">Incoming Message object</param>
        /// <returns>Returns a response text message to the user</returns>
        [HttpPost]
        public TwiMLResult Incoming(SmsRequest incomingMessage)
        {
            
            var response = new MessagingResponse();
            DBContext db = new DBContext();
           
            if (CheckValidCustomer(incomingMessage.From))
            {
                Customer customer = (from c in db.Customers where incomingMessage.From.Trim().Equals(c.PhoneNumber) select c).Single();

                var customerID = customer.CustID;
                MessageController.AddIncomingMessage(customer, incomingMessage.Body, incomingMessage.SmsSid);
                var latestMsg = (from m in db.Messages where m.CustID == customerID orderby m.Date descending select m).FirstOrDefault();

                if (latestMsg != null)
                {
                    if (latestMsg.Type == MessageType.Request || latestMsg.Type == MessageType.Invalid || latestMsg.Type == MessageType.Inbound)
                    {
                        String messageText;
                        if (IsMessageValid(incomingMessage.Body))
                        {
                            Order order = OrderController.CreateOrder(customer);
                            messageText = "Success! Your Order has been placed \n Your invoice number is: " + order.OrderID;
                            response.Message(messageText);
                            MessageController.CreateOutgoingMessage(customer, messageText, MessageType.Confirmation);
                            return TwiML(response);
                        }
                        else
                        {
                            //Error Message asking users to resend another message
                            messageText = "Invalid Message, Blah blah blah";
                            response.Message(messageText);
                            MessageController.CreateOutgoingMessage(customer, messageText, MessageType.Invalid);
                            return TwiML(response);
                        }
                    }
                }
            }

            //Return an error msg if message came from a user not in the table
            response.Message(""); 
            return TwiML(response);
        }

        /// <summary>
        /// Checks if customer's reponse is a valid response to order a new set of paper roll
        /// </summary>
        /// <param name="message">Customer's response message</param>
        /// <returns></returns>
        public bool IsMessageValid(string message)
        {
            string unformattedMsg = (message.Trim()).ToLower();

            switch (unformattedMsg)
            {
                case "yes":
                    return true;
                case "no":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the phone number is in the customer table and sees if they are subsscribed
        /// </summary>
        /// <param name="phoneNumber">Phone number that sent the message</param>
        /// <returns>If its a valid user</returns>
        public bool CheckValidCustomer(string phoneNumber)
        {
            //TODO: check if phone number is valid format
            using (DBContext db = new DBContext())
            {
                var isValid = (from c in db.Customers where phoneNumber.Equals(c.PhoneNumber) && c.IsSubscribed == true select c).FirstOrDefault();
                if (isValid == null)
                {
                    return false;
                }
                else
                    return true;
            }
        }
    }
}