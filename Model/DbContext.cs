using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class DBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}