using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Complaint
    {
        [Required]
        public int ComplaintId { get; set; }
        [Required]
        [Display(Name = "Lodge Time")]

        public DateTime LodgedOn { get; set; }
        [Display(Name = "Solved?")]
        public bool IsSolved { get; set; }
        [Display(Name = "Technician Assigned?")]
        public bool IsAssigned { get; set; }
        public string Status { get; set; }
        [Required]
        [Display(Name = "Issue")]
        public string Issue { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [Required]
        [Display(Name = "Lodged By")]
        public string LodgedBy { get; set; }
        [Required]
        [Display(Name="Device")]
        public int DeviceId { get; set; }
        [Display(Name = "Technician")]
        public string AssignedTo { get; set; }
    }
}