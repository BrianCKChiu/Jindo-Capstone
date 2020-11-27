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
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,16}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Your password does not follow the password complexity rules")]
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
        public EmpType Type
        {
            get; set;
        }
    }

    public enum EmpType
    {
        Admin = 1,
        Standard = 0
    }
}

