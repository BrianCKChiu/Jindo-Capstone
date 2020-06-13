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
                return View("Index");
            }
 
            using (DBContext dbas = new DBContext())
            {
                dbas.CreateLocalDB();
                List<Employee> checkIfExists = Employee.CheckIfExists(x.UserName, x.Password);
                int rowCount = checkIfExists.Count;
                if (rowCount == 0)
                {
                    ViewBag.ErrorMessage = "Access denied. User name and password don't match";
                    return View("Index");
                }
                else if (rowCount >= 2)
                {
                    ViewBag.ErrorMessage = "Programming error: there is more than record in the database with this user name and password.";
                    return View("Index");
                }
                else
                {
                    Session["userName"] = x.UserName.Trim();
                    Session["empType"] = checkIfExists[0].EmpType;
                    return RedirectToAction("Index", "Home");
                }
                
            }
        }
    }
}