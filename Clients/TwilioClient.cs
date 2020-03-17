using Jindo_Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace Jindo_Capstone.Clients
{
    public class TwilioClient
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
                    to: msg.customer.PhoneNumber,
                    client: _client);
        }
    }
}