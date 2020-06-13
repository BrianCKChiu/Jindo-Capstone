using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
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
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone #")]
        [RegularExpression(@"^\d{10}$",
         ErrorMessage = "Phone Numbers must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Employee Type")]
        public EmpType EmpType { get; set; }
        
        public static List<Employee> CheckIfExists(string userName)
        {
            DBContext dbas = new DBContext();
            List<Employee> y = new List<Employee>();
            y = (from emp in dbas.Employees
                 where emp.UserName.Equals(userName.Trim())
                 select emp).ToList();
            return y;

        }
        public static List<Employee> CheckIfExists(string userName, string password)
        {
            List<Employee> x = new List<Employee>();
            x = CheckIfExists(userName);
            List<Employee> y = new List<Employee>();
            y = (from emp in x where emp.Password.Equals(password.Trim()) select emp).ToList();
            return y;
        }
  
    }

    public enum EmpType
    {
        Standard, Admin
    }
}

