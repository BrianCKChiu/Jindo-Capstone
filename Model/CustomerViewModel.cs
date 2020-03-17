using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            Customers = new List<Customer>();
        }
    }
}