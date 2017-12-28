using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int ReferenceNumber { get; set; } //AppointmentId

        [Display(Name = "Channel Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ChannelDate { get; set; }

        [Display(Name = "Channel Time"), DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ChannelTime { get; set; }

        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }

        [Display(Name = "Channel Number")]
        public string ChannelNumber { get; set; } //Position in Queue & Room

        public int ChannelNum { get; set; }

        [Display(Name = "Channel Fee")]
        public double ChannelFee { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }

        public List<Doctor> Doctors { get; set; }

        public List<Patient> Patients { get; set; }

        public List<Drug> Drugs { get; set; }

        

        //public string Name { get; set; }

        public string PatientId { get; set; }
        public string DoctorId { get; set; }

        public bool IsDateSubmitted { get; set; }
        public bool IsConfirmed { get; set; }
        public string ViewBagMessage { get; set; }
        public bool IsPaid { get; set; }
        public bool IsChanneled { get; set; }
        public bool IsPrescriptionGenerated { get; set; }
    }
}