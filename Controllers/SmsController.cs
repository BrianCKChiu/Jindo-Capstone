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

        public void SendMessage(Message msg)
        {
            //sends message
            MessageResource.Create(
                body: msg.MessageContent,
                from: _twilioNumber,
                to: msg.Customer.PhoneNumber,
                client: _client);
        }

        public TwiMLResult Incoming(SmsRequest incomingMessage)
        {



            MessagingResponse orderMsg = new MessagingResponse();
            DBContext db = new DBContext();
            if(CheckValidCustomer(incomingMessage.From))
            {
                var customer = (from c in db.Customers where incomingMessage.From == c.PhoneNumber select c).Single();
                //add error checking if null 
                MessageController.AddIncomingMessage(customer, incomingMessage.Body);

                //selects the latest message sent/recieved by the customer
                var latestMsg = (from m in db.Messages where m.CustID == customer.CustID orderby m.Date descending select m).Single(); 

                if(latestMsg.Msg == MessageType.Request)
                {
                    if (IsMessageValid(incomingMessage.Body))
                    {
                        Order order = OrderController.CreateOrder(customer);
                        String messageText = "Success! Your Order has been placed \n Your invoice number is: " + order.InvoiceNumber;
                        orderMsg.Message(messageText);
                        MessageController.CreateMessage(customer, messageText);
                        return TwiML(orderMsg);
                    }
                    else
                    {
                        //Error Message asking users to resend another message
                        String messageText = "Invalid Message, Blah blah blah";
                        orderMsg.Message(messageText);
                        MessageController.CreateMessage(customer, messageText);
                        return TwiML(orderMsg);
                    }
                }
            }
            MessagingResponse response = new MessagingResponse();
            response.Message("Message recieved"); // temp
            return TwiML(response);

        }

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

        //wip
        public bool CheckValidCustomer(string phoneNumber)
        {
            //TODO: check if phone number is valid format

            using (DBContext db = new DBContext())
            {
                var isValid = (from c in db.Customers where phoneNumber.Equals(c.PhoneNumber) select c).FirstOrDefault();
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