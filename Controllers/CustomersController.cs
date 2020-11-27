using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jindo_Capstone.Workers;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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

            TempData["ModalMsg"] = "Message is sent";
            return Redirect("~/Customers/Index");
        }

        [HttpPost]
        public ActionResult BatchSubmit(CustomerList cl)
        {
            TempData["ModalMsg"] = "";
            // Used to store the list of customer names for display in the CustomMessageBox
            string message = "";

            // foreach loops through each of the customer items in CustomerList which is a list of all the checkboxes on the page
            foreach (var item in cl.customers)
            {
                // Each item in customers is checked to determine if the checkbox item has been checked, sends text if it has been.
                if (item.IsChecked) {
                    SendTextMessageJob.Execute(item.CustID);
                    message = message + "\n" + item.ContactName;
                }
            }
            Debug.WriteLine("Before");
            //Creates a success popup for batch submit
            TempData["ModalMsgHeading"] = "Batch Successful";
            TempData["ModalMsg"] = "Batch Submission message has been sent to the following clients:" + message;
            Debug.WriteLine("After");
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

        public ActionResult Unsubscribe(int id)
        {
            TempData["ModalMsg"] = "";
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Unsubscribe the customer
                customer.IsSubscribed = false;
                int result = db.SaveChanges();
                if (result == 1)
                    TempData["ModalMsg"] = "Successfully Unsubscribe";
            }
            return Redirect("~/Customers/Index");
        }

        public ActionResult Subscribe(int id)
        {
            TempData["ModalMsg"] = "";
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Subscribe the customer
                customer.IsSubscribed = true;
                int result = db.SaveChanges();
                if (result == 1)
                    TempData["ModalMsg"] = "Successfully Subscribe";
            }
           

            return Redirect("~/Customers/Index");
        }

        
    }


}