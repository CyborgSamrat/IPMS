using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Location
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int Room { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Column { get; set; }
        [Display(Name = "Device")]

        public int Device { get; set; }
    }
}