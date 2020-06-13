using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=Jindo-Capstone") {}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
  
        public void CreateLocalDB() {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DBContext>());

            ////Add Test Data
            if (Employees.Count() == 0) {
                List<Employee> initialData = new List<Employee>() {
                new Employee(){
                    UserName="admin1",
                    Password="Snickerdoodles@$99",
                    PhoneNumber="9051234568",
                    EmpType=EmpType.Admin,
                    Name="Jonny Admin"
                },
                new Employee(){
                    UserName="standard1",
                    Password="Snickerdoodles@$99",
                    PhoneNumber="9051234567",
                    EmpType=EmpType.Standard,
                    Name="Jonny Standard"
                }
            };
                foreach (Employee x in initialData)
                {
                    if (Employee.CheckIfExists(x.UserName).Count == 0)
                    {
                        Employees.Add(x);
                        SaveChanges();
                    }
                }

            }

        }


    }
}