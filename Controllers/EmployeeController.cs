﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jindo_Capstone.Models;

namespace Jindo_Capstone.Controllers
{
    public class EmployeeController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Employee
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            if (!Session["empType"].Equals(Jindo_Capstone.Models.EmpType.Admin))
            {
                return RedirectToAction("Index");//this is to block non admin employees
            }
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,Name,PhoneNumber,EmpType")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                int rowCount = Employee.CheckIfExists(employee.UserName).Count;
                if (rowCount == 0)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (rowCount==1)
                {
                    Debug.WriteLine("Employee already exists with this user name. Unable to change.");
                }
                else {
                    Debug.WriteLine("Programming error: there is more than record in the database with this user name and password.");
                }

              
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Password,Name,PhoneNumber,EmpType")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(string id)
        {
            if (!Session["empType"].Equals(Jindo_Capstone.Models.EmpType.Admin))
            {
                return RedirectToAction("Index");//this is to block non admin employees
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            else if (employee.UserName.Equals(Session["userName"]))
            {
                Debug.WriteLine("Users are not permitted to delete themselves from the application. Therefore the delete button will not work in this instance.");
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee.UserName.Equals(Session["userName"]))
            {
                return Delete(id);
            }
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}