using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models.ViewModels
{
    public class PrescriptionViewModel
    {
        [Display(Name ="Prescription Identifier")]
        public int PrescriptionId { get; set; }

        [Display(Name = "Issued Date"), DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }

        [Required, ScaffoldColumn(false)]
        public string DoctorId { get; set; }

        [Required, ScaffoldColumn(false)]
        public string PatientId { get; set; }

        [Display(Name ="Patient Name")]
        public string PatientName { get; set; }

        [ScaffoldColumn(false)]
        public string Disease { get; set; }

        [ScaffoldColumn(false)]
        public string Description { get; set; }

        [Required, ScaffoldColumn(false)]
        public List<Drug> Drugs { get; set; }

        public List<DrugItem> DrugItems { get; set; }

        //public DrugItem DrugItem { get; set; }
        public Drug Drug { get; set; }

        public int Quantity { get; set; }

        [Required, Display(Name ="Appointment Reference Number")]
        public int AppointmentReference { get; set; }

        [Required, ScaffoldColumn(false)]
        public bool IsPaid { get; set; }

        [Required, ScaffoldColumn(false)]
        public bool IsCleared { get; set; }

        [DataType(DataType.Currency), Range(0, 3000)]
        public double SubTotal { get; set; }

        [Display(Name ="Payment Type"), Required]
        public PaymentType PaymentType { get; set; }

    }
}