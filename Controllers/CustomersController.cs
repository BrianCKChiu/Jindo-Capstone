﻿using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jindo_Capstone.Workers;

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
                int result = db.SaveChanges();
                if (result == 1)
                    TempData["ModalMsg"] = "Successfully Unsubscribe";
            }
            return Redirect("~/Customers/Index");
        }

        public ActionResult Subscribe(int id)
        {
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