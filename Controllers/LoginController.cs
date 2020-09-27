using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jindo_Capstone.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Employee x)
        {
            if (x.UserName == null || x.Password == null || x.Password.Trim().Length<1 || x.UserName.Trim().Length<1)
            {
                ViewBag.ErrorMessage = "Both password and user name are required before continuing.";
                return View("Index");
            }
            try
            {
                //checks if credentials are valid
                if(EmployeesController.CheckCredentials(x.UserName, x.Password))
                {
                    using (DBContext db = new DBContext())
                    {
                        Session["userName"] = x.UserName.Trim();
                        Session["empType"] = EmployeesController.GetEmployeeType(x.UserName);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    throw new NullReferenceException("Something went wrong! Please contact support");
                }

            } 
            //catches any errors retrieving or processing employee data from the database
            catch(NullReferenceException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Index");
            }

        }
    }
}