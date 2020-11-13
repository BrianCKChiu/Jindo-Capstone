using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jindo_Capstone.Workers;
using System.Diagnostics;
using System.Text;

namespace Jindo_Capstone.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            CustomerViewModel cus = new CustomerViewModel();

            using (var db = new DBContext())
            {
                var cust =  (from c in db.Customers select c).ToList();
                
                foreach(var i in cust)
                {
                    cus.Customers.Add(i);
                }
                //ViewBag.Customers = customers;
            }
            return View(cus);
        }

        public ActionResult Send(int id)
        {
            SendTextMessageJob.Execute(id);
            return Redirect("~/Customers/Index");
        }


        // Batch Submit - Responds to the form and the generated html tags
        [HttpPost]
        public ActionResult BatchSubmit(CustomerList cl)
        {

            Debug.WriteLine("This is the Batch Submit");

            List<int> selItems = new List<int>();
            foreach (var item in cl.customers)
            {
                if (item.IsChecked) {
                    selItems.Add(item.CustID);
                    Debug.WriteLine("Added: " + item.CustID);
                }
            }

            /* 
            For actually sending the batch
            foreach (var item in selItems) {
                SendTextMessageJob.Execute(id);
            }*/

            return Redirect("~/Customers/Index");
        }

        /// <summary>
        /// Checks if the phone number is in the customer table and sees if they are subsscribed
        /// </summary>
        /// <param name="phoneNumber">Phone number that sent the message</param>
        /// <returns>If its a valid user</returns>
        public static bool CheckValidCustomer(string phoneNumber)
        {
            //TODO: check if phone number is valid format
            using (DBContext db = new DBContext())
            {
                var isValid = (from c in db.Customers where phoneNumber.Equals(c.PhoneNumber) && c.IsSubscribed == true select c).FirstOrDefault();
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