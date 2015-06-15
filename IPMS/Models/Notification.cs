using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Notification
    {
        [Required]
        public int NotificationId { get; set; }
        [Required]
        [Display(Name = "Send Time")]

        public DateTime SendTime { get; set; }

        [Display(Name = "Issue")]
        public string Issue { get; set; }
        public bool IsSeen { get; set; }

        [Required]

        public string Sender { get; set; }
        [Required]
        public string Receiver { get; set; }
    }
}