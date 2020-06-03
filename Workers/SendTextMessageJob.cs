using Jindo_Capstone.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jindo_Capstone.Clients;
using Jindo_Capstone.Controllers;

namespace Jindo_Capstone.Workers
{
    public static class SendTextMessageJob
    {


        public static void Execute()
        {
            SubscribedCustomer().ForEach(c => MessageController.CreateReorderMessage(c));
        }
        public static void Execute(int id)
        {
            SubscribedCustomer(id).ForEach(c => MessageController.CreateReorderMessage(c));
        }



        public static IEnumerable<Customer> SubscribedCustomer()
        {
            return new CustomerFinder().FindSubscribedCustomers();
        }
        public static IEnumerable<Customer> SubscribedCustomer(int id)
        {
            return new CustomerFinder().FindSubscribedCustomers(id);
        }

    }
}