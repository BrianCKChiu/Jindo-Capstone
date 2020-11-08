using Jindo_Capstone.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;
using System.Web.Razor.Generator;

namespace Jindo_Capstone.Controllers
{
    public class OrderController : Controller
    {
        public static Order CreateOrder(Customer customer)
        {
            DBContext db = new DBContext();
            Order order = new Order
            {
               OrderID = GenerateInvoiceNumber(1),
               CustID = customer.CustID,
               Date = DateTime.Now,
               OrderAmount = 10,
               Status = OrderStatus.NotShipped
            };

            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }

        private static int GenerateInvoiceNumber(int id)
        {
            //(id : number + string)

            return 0;
        }


        private DBContext db = new DBContext();

        // GET: OrdersView
        public ActionResult Index(int? pageNumber, int? pageSize)
        {
            return View(db.Orders.ToList().ToPagedList(pageNumber ?? 1, pageSize ?? 10));
        }

        // GET: OrdersView/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: OrdersView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersView/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceNumber,CustomerID,Date,Status,OrderAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: OrdersView/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersView/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceNumber,CustomerID,Date,Status,OrderAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: OrdersView/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public static Customer GetCustomer(Order order)
        {
            DBContext db = new DBContext();
            return (from c in db.Customers where order.CustID == c.CustID select c).Single();
        }

        public static bool IsStatusValid(Order order)
        {
            if (Enum.IsDefined(typeof(OrderStatus), order.Status))
                return true;
            else
                return false;
        }

        public static void ChangeOrderStatus(Order order, OrderStatus status)
        {
            order.Status = status;
        }

        public static Order UpdateOrder(Order order)
        {
            using (DBContext db = new DBContext())
            {

                Order queryOrder = (from o in db.Orders where o.OrderID == order.OrderID select o).SingleOrDefault();
                queryOrder.Status = order.Status;

                if (order.TrackingNumber != null)
                {
                    queryOrder.TrackingNumber = order.TrackingNumber;
                }

                db.SaveChanges();
                return queryOrder;
            }
        }

        public static bool CheckIfValidOrder(Order order)
        {
            using (DBContext db = new DBContext())
            {
                Order queryOrder = (from o in db.Orders where o.OrderID == order.OrderID select o).SingleOrDefault();

                if (queryOrder == null)
                    return false;
                else
                    return true;
            }
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

