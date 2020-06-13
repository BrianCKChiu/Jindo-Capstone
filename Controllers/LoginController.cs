﻿using Jindo_Capstone.Models;
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
            using (DBContext dbas = new DBContext())
            {
                dbas.CreateLocalDB();
                List<Employee> checkIfExists = Employee.CheckIfExists(x.UserName, x.Password);
                int rowCount = checkIfExists.Count;
                if (rowCount == 0)
                {
                    Debug.WriteLine("Access denied. User name and password don't match");
                    return View("Index",x);
                }
                else if (rowCount >= 2)
                {
                    Debug.WriteLine("Programming error: there is more than record in the database with this user name and password.");
                    return View("Index", x);
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