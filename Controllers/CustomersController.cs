using Jindo_Capstone.Model;
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
            return Redirect("~");
        }
    }
}