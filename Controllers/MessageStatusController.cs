using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio.Rest.Api.V2010.Account;

namespace Jindo_Capstone.Controllers
{
    public class MessageStatusController : ApiController
    {
        ///api/MessageStatus/Update
        [HttpPost]
        public void Update(MessageResource value)
        {
            Console.WriteLine(value);

        }
    }
}
