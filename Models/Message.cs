using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jindo_Capstone.Models
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

        //property needs to be renamed
        [Required, Column("MessageType")]
        public MessageType Type { get; set; }
        public string MessageSID { get; set;}
        public int? CustID { get; set; }
        [NotMapped]
        public virtual Customer Customer { get; set; }
    }

    public enum MessageType
    {
        //incoming messages
        Inbound = 0,  Confirmation = 2, Invalid = 4, Decline = 6, Unsubscribe = 7,

        //outgoing messages
        Outbound = 1, Request = 3, Error = 5
    }
}