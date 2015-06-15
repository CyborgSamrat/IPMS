using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Device
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        [Display(Name = "Device Status")]
        public string Status { get; set; }
        [Required]
        [Display(Name="Location")]
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}