using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public DateTime IssuedDate { get; set; }

        public Doctor Issuer { get; set; }

        public Patient Patient { get; set; }

        public string Disease { get; set; }

        public string Description { get; set; }

        public List<DrugItem> DrugItems { get; set; }

        public int AppointmentReference { get; set; }

        public bool IsPaid { get; set; }

        public bool IsCleared { get; set; }

        public string Feedback { get; set; }

        public bool FeedbackSubmitted { get; set; }
    }

    public class DrugItem
    {
        public int Id { get; set; }
        public Drug Drug { get; set; }

        [DataType(DataType.Currency), Range(0, 3000)]
        public double Price { get; set; }

        [Display(Name = "Quantity")]
        public int OrderQuantity { get; set; }

        [DataType(DataType.Currency), Range(0, 3000)]
        public double Total { get; set; }

        [ScaffoldColumn(false)]
        public bool IsPackedToPrescrition { get; set; }

        [ScaffoldColumn(false), Required] //AppointmentId
        public int ReferenceNumber { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public Prescription Prescription { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public StaffMember Cashier { get; set; }
    }

    public enum PaymentType { Credit_Card = 1, Debit_Card, Cash, Cheque}
}