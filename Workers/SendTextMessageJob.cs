using Jindo_Capstone.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jindo_Capstone.Clients;
using Jindo_Capstone.Controllers;
using System.IO;

namespace Jindo_Capstone.Workers
{
    public static class SendTextMessageJob
    {


        public static void Execute()
        {
            StreamWriter sw = new StreamWriter("F:\\College\\System Analyst\\Semester 6\\INFO3001 Capstone Project\\Hangfire\\HangfireLog.txt", true);
            sw.WriteLine(" --- " + DateTime.Now + "\n");
            sw.Flush();
            sw.Close();

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