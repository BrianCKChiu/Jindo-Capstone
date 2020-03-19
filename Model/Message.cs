using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int MessageID { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string MessageContent { get; set; }
        public int? CustID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}