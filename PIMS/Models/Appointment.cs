using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models
{
    public class Appointment
    {
        public int Id { get; set; } //Reference Number

        [Required]
        public DateTime ChannelDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string PatientId { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [Display(Name = "Channel Number")]
        public int ChannelNumber { get; set; } //Position in Queue

        [Display(Name = "Channel Fee")]
        public double ChannelFee { get; set; }

        [ScaffoldColumn(false)]
        public bool IsPaid { get; set; }

        [ScaffoldColumn(false)]
        public bool IsChanneled { get; set; }
    }
}