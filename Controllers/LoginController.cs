using Jindo_Capstone.Models;
using System;
using System.Collections.Generic;
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
            using (DBContext dbas = new DBContext())
            {
                var checkRowCount = from emp in dbas.Employees
                                    where emp.UserName.Equals(x.UserName.Trim()) && emp.Password.Equals(x.Password.Trim())
                                    select emp;
                
                int rowCount = checkRowCount.ToList().Count();
       
                if (rowCount == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Access denied. User name and password don't match");
                    return View("Index", x);
                }
                else if (rowCount >= 2)
                {
                    System.Diagnostics.Debug.WriteLine("Programming error: there is more than record in the database with this user name and password.");
                    return View("Index", x);
                }
                else
                {
                    Session["userName"] = x.UserName.Trim();
                    Session["empType"] = checkRowCount.ToList().First().EmpType;
                    return RedirectToAction("Index", "Home");
                }
                
            }
        }
    }
}