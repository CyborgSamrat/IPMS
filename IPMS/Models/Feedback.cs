using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Feedback
    {
        [Required]
        public int FeedbackId { get; set; }
        [Required]
        [Display(Name = "Issue")]
        public string Issue { get; set; }
        [Display(Name = "Body")]
        public string Body { get; set; }
        [Required]
        [Display(Name = "Given By")]
        public string GivenBy { get; set; }

        [Display(Name = "Complaint")]
        public int ComplaintId { get; set; }
    }
}