using Jindo_Capstone.Model;
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
                                    where emp.userName.Equals(x.userName.Trim()) && emp.password.Equals(x.password.Trim())
                                    select emp;
                
                int rowCount = checkRowCount.ToList().Count();
                if (rowCount == 0)
                {
                    x.errorMessage = "Access denied. User name and password don't match";
                    return View("Index", x);
                }
                else if (rowCount >= 2)
                {
                    x.errorMessage = "Programming error: there is more than record in the database with this user name and password.";
                    return View("Index", x);
                }
                else
                {
                    Session["userName"] = x.userName.Trim();
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}