using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Twilio.Clients;

namespace Jindo_Capstone
{
    public class TwilioClient
    {
        private readonly ITwilioRestClient _client;
        private readonly string _tokenID = WebConfigurationManager.AppSettings["AuthToken"];
        private readonly string _accountSid = WebConfigurationManager.AppSettings["AccountSid"];
        private readonly string _twilioNumber = WebConfigurationManager.AppSettings["TwilioNumber"];
    }
}