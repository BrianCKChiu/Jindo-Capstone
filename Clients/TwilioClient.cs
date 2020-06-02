using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.AspNet.Common;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;

namespace Jindo_Capstone.Clients
{
    public class TwilioClient : TwilioController
    {
        private readonly ITwilioRestClient _client;
        private readonly string _tokenAuth = WebConfigurationManager.AppSettings["AuthToken"];
        private readonly string _accountSid = WebConfigurationManager.AppSettings["AccountSid"];
        private readonly string _twilioNumber = WebConfigurationManager.AppSettings["TwilioNumber"];

        public TwilioClient()
        {
            _client = new TwilioRestClient(_accountSid, _tokenAuth);
            
        }
        public TwilioClient(TwilioRestClient client)
        {
            _client = client;
        }

        public void SendMessage(Message msg)
        {
                MessageResource.Create(
                    body: msg.MessageContent,
                    from: _twilioNumber,
                    to: msg.Customer.PhoneNumber,
                    client: _client);
        }

        public TwiMLResult Index(SmsRequest incomingMessage)
        {
            MessagingResponse resposne = new MessagingResponse();
            resposne.Message("The copy cat says: " +incomingMessage.Body);
            return TwiML(resposne);
        }

    }
}