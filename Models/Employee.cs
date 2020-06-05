using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
{
    public class Employee
    {
  
        [Required, Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public EmpType EmpType { get; set; }
    }

    public enum EmpType
    {
        Admin, Employee
    }
}

