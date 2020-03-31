﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jindo_Capstone.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (Session["userName"] == null)
            {
                return Logout();
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["userName"] = null;
            return RedirectToAction("Index", "Login");

        }
    }
}