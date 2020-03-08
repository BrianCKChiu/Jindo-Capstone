using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Model
{
    public class Message
    {
        [Required]
        public int MessageID { get; set; }
        public int ReplyID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public string MessageContent { get; set; }

    }
}