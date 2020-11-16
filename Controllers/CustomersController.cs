using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jindo_Capstone.Workers;
using System.Windows.Forms;

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
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Unsubscribe the customer
                customer.IsSubscribed = false;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = customer.ContactName + " has been unsubscribed.";
                    string caption = "Success";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult dialog;

                    // Displays the MessageBox.
                    dialog = MessageBox.Show(message, caption, buttons);
                    /*
                    if (dialog == System.Windows.Forms.DialogResult.OK)
                    {
                        // Closes the parent form.
                        this.Close();
                    }*/
                }
                else
                {
                    string message = "There has been an internal server error, please contact your system administrator.";
                    string caption = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult dialog;

                    // Displays the MessageBox.
                    dialog = MessageBox.Show(message, caption, buttons);
                }
            }
            
            return Redirect("~/Customers/Index");
        }

        public static void SelfUnsubscribe(int id)
        {
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Unsubscribe the customer
                customer.IsSubscribed = false;
                db.SaveChanges();
            }
        }

        public ActionResult Subscribe(int id)
        {
            // Grab the customer
            using (DBContext db = new DBContext())
            {
                var customer = (from c in db.Customers where c.CustID == id select c).FirstOrDefault();
                // Subscribe the customer
                customer.IsSubscribed = true;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = customer.ContactName + " has been subscribed.";
                    string caption = "Success";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult dialog;

                    // Displays the MessageBox.
                    dialog = MessageBox.Show(message, caption, buttons);
                    /*
                    if (dialog == System.Windows.Forms.DialogResult.OK)
                    {
                        // Closes the parent form.
                        this.Close();
                    }*/
                }
                else
                {
                    string message = "There has been an internal server error, please contact your system administrator.";
                    string caption = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult dialog;

                    // Displays the MessageBox.
                    dialog = MessageBox.Show(message, caption, buttons);
                }
            }

            return Redirect("~/Customers/Index");
        }
    }


}