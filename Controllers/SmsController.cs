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
using System.Security;
using WebGrease.Configuration;

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

            return msgObject.Sid;
        }


        /// <summary>
        /// Process incomming messages received and returns a response to the sender
        /// [Sms/Incoming]
        /// </summary>
        /// <param name="incomingMessage">Incoming Message object</param>
        /// <returns>Returns a response text message to the user</returns>
        [HttpPost]
        public TwiMLResult Incoming(SmsRequest incomingMessage)
        {
            var response = new MessagingResponse();
            DBContext db = new DBContext();
           
            if (CustomersController.CheckValidCustomer(incomingMessage.From))
            {
                Customer customer = (from c in db.Customers where incomingMessage.From.Trim().Equals(c.PhoneNumber) select c).Single();
                var customerID = customer.CustID;
                var IncomingMessagetype = MessageController.DetermineMessageType(incomingMessage.Body, customerID);
                String messageBody = incomingMessage.Body;
                
                MessageController.AddIncomingMessage(customer, messageBody, incomingMessage.SmsSid);

                switch (IncomingMessagetype)
                {
                    case MessageType.Confirmation:
                        Order order = OrderController.CreateOrder(customer);
                        response.Message(WebConfigurationManager.AppSettings["Confirmation"] + order.OrderID);
                        break;
                    case MessageType.Invalid:
                        response.Message(WebConfigurationManager.AppSettings["Invalid"]);
                        break;
                    case MessageType.Error:
                        response.Message(WebConfigurationManager.AppSettings["Error"]);
                        break;
                    case MessageType.Decline:
                        response.Message(WebConfigurationManager.AppSettings["Decline"]);
                        break;
                    case MessageType.Unsubscribe:
                        response.Message(WebConfigurationManager.AppSettings["Unsubscribe"]);
                        break;
                    default:
                        response.Message(WebConfigurationManager.AppSettings["Error"]);
                        break;
                }
                MessageController.CreateOutgoingMessage(customer, WebConfigurationManager.AppSettings[IncomingMessagetype.ToString()], IncomingMessagetype);
                return TwiML(response);
            } 
            else
            {
                //Return an error msg if message came from a user not in the table
                response.Message(WebConfigurationManager.AppSettings["UnknowNumber"]);
                return TwiML(response);
            }
        }
    }
}