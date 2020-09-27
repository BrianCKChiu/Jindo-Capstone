using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Jindo_Capstone.Models;
using Twilio.Rest.Studio.V2.Flow;

namespace Jindo_Capstone.Controllers
{
    public class EmployeesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
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

        // GET: Employees/Create
        public ActionResult Create()
        {
            if (!(Session["empType"].Equals("admin")))
            {
                return RedirectToAction("Index");
            }
            
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userName,password,empType")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var checkForEmployee = from emp in db.Employees
                                    where emp.UserName.Equals(employee.UserName.Trim())
                                    select emp;
                int rowCount = checkForEmployee.ToList().Count();
                if (rowCount == 0)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else {
                    //employee.errorMessage = "The user name you entered already exists for another employee. Please try again";
                }
               
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            return RedirectToAction("Index");
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userName,password,empType")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (!(Session["empType"].Equals("admin"))) {
                return RedirectToAction("Index");
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
            else if (employee.UserName.Equals(Session["userName"])) {
               // employee.errorMessage = "Users are not permitted to delete themselves from the application. Therefore the delete button will not work in this instance.";
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee.UserName.Equals(Session["userName"])) {
                return Delete(id);
            }
             db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*
        * <summary> 
        *   Queries for an employee in the database
        * </summary>
        * <param name="username">employee's username</param>
        * <returns>
        *   Employee's data or a null object
        * </returns>
        */
        public static Employee GetEmployee(string username) 
        {
            DBContext db = new DBContext();
            List<Employee> employeeList = 
                (from e in db.Employees
                 where e.UserName.Equals(username.Trim())
                 select e).ToList();

            //checks if there's more than one employee found
            if (employeeList.Count > 1)
            {
                //todo: add a log statement that indicates issue
                return null;
            }
            //if no employee is found
            else if(employeeList.Count <= 0)
            {
                //todo: add a log statement that indicates issue
                return null;
            }
            Employee emp = employeeList.Single();
            return emp;
        }

        public static bool CheckCredentials(string username, string password)
        {
            Employee emp = GetEmployee(username);
            if(emp == null)
            {
                throw new NullReferenceException("Invalid Username/Password");
            }
            /*
             * decode password
             */

            //check if password is valid
            if(emp.Password != password)
            {
                throw new NullReferenceException("Invalid Username/Password");
            }
            return true;
        }

        public static EmpType GetEmployeeType(string username)
        {
            DBContext db = new DBContext();
            return (from e in db.Employees where e.UserName == username select e.Type).Single();
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
