//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jindo_Capstone.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Employee
    {
   
        public string empType { get; set; }
        [Display(Name = "User Name")]
        [DataType(DataType.Text), Key]
        public string userName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [NotMapped]
        public string errorMessage { get; set; }
    }
}
