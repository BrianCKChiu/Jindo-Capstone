using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class Employee
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public EmpType EmpType { get; set; }
    }

    public enum EmpType
    {
        Admin, Employee
    }
}

