using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=Jindo-Capstone") 
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DBContext>());
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public List<Employee> CheckIfExists(string userName) {
            List<Employee> y = new List<Employee>();
            y= (from emp in this.Employees
               where emp.UserName.Equals(userName.Trim())
               select emp).ToList();
            return y;
        }
        public List<Employee> CheckIfExists(string userName, string password)
        {
            List<Employee> y = new List<Employee>();
            y = (from emp in this.Employees
                 where emp.UserName.Equals(userName.Trim()) && emp.Password.Equals(password.Trim())
                 select emp).ToList();
            return y;
        }

    }
}