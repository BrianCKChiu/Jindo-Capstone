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
                    return Request.CreateResponse(HttpStatusCode.BadRequest,  "Status update failed, order not found.");
                }

                if(OrderController.IsStatusValid(order))
                {
                    Order updatedOrder = OrderController.UpdateOrder(order);
                    MessageController.CreateOutgoingMessage(OrderController.GetCustomer(updatedOrder), "Your order has been ship! Your tracking number is: " + updatedOrder.TrackingNumber, MessageType.Outbound);

                    return Request.CreateResponse(HttpStatusCode.OK, "Message has been sent!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Message Type Invalid");
                }
        }
    }

}
