using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio.Rest.Api.V2010.Account;
using Jindo_Capstone.Models;
using System.Data;
using System.Web.Configuration;

namespace Jindo_Capstone.Controllers
{
    public class UpdateController : ApiController
    {
        /*
         * {
         * OrderID: 9999,
         * OrderStatus: Shipped,
         * Date: Oct 10th 2020,
         * TrackingNumber: es9dfies9fes9 (Optional)
         * }
         */ 
        [HttpPost]
        public HttpResponseMessage UpdateOrder(Order order)
        {

                if(!OrderController.CheckIfValidOrder(order))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 
                        WebConfigurationManager.AppSettings["OrderNotFound"]);
                }

                if(OrderController.IsStatusValid(order))
                {
                    Order updatedOrder = OrderController.UpdateOrder(order);

                    MessageController.CreateOutgoingMessage(OrderController.GetCustomer(updatedOrder),
                        WebConfigurationManager.AppSettings["OrderShippedMsg"] 
                        + updatedOrder.TrackingNumber, MessageType.Outbound);

                    return Request.CreateResponse(HttpStatusCode.OK, WebConfigurationManager.AppSettings["MsgSent"]);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 
                        WebConfigurationManager.AppSettings["MsgInvalid"]);
                }
        }
    }

}
