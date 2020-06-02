using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=Jindo-Capstone") { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}