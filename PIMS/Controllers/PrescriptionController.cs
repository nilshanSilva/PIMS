using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMS.Models;
using PIMS.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace PIMS.Controllers
{
    public class PrescriptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public PrescriptionController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Prescription
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePrescription(int appointId)
        {
            Appointment appointment = db.Appointments.Find(appointId);
            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.AppointmentReference = appointId;
            PresVM.Drugs = db.Drugs.Take(5).ToList();
            PresVM.DoctorId = appointment.DoctorId;
            PresVM.PatientId = appointment.PatientId;
            return View(PresVM);
        }

        public ActionResult AddDrugItem(int drugId, int Quantity, int appointId)
        {
            DrugItem drugItemInDb = db.DrugItems.Where(r => r.ReferenceNumber == appointId)
                .Where(d => d.Drug.Id == drugId).FirstOrDefault();
            if (drugItemInDb != null)
            {
                drugItemInDb.OrderQuantity = drugItemInDb.OrderQuantity + Quantity;
                drugItemInDb.Total = drugItemInDb.OrderQuantity * drugItemInDb.Price;
            }
            else
            {
                DrugItem drugItem = new DrugItem();
                drugItem.Drug = db.Drugs.Find(drugId);
                drugItem.Price = drugItem.Drug.RetailPrice;
                drugItem.OrderQuantity = Quantity;
                drugItem.Total = drugItem.Price * Quantity;
                drugItem.ReferenceNumber = appointId;
                db.DrugItems.Add(drugItem);
            }
            db.SaveChanges();

            Appointment appointment = db.Appointments.Find(appointId);
            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.AppointmentReference = appointId;
            PresVM.Drugs = db.Drugs.Take(5).ToList();
            PresVM.DoctorId = appointment.DoctorId;
            PresVM.PatientId = appointment.PatientId;

            PresVM.DrugItems = db.DrugItems.Where(x => x.ReferenceNumber == appointId).ToList();
            return View("CreatePrescription", PresVM);
        }

        public ActionResult SelectDrug(int appointId, int drugId)
        {
            Appointment appointment = db.Appointments.Find(appointId);
            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.AppointmentReference = appointId;
            PresVM.Drugs = db.Drugs.Take(5).ToList();
            PresVM.DoctorId = appointment.DoctorId;
            PresVM.PatientId = appointment.PatientId;

            PresVM.Drug = db.Drugs.Find(drugId);
            return View("CreatePrescription", PresVM);
        }

        public ActionResult FinishPrescription(PrescriptionViewModel pvm)
        {
            List<DrugItem> DrugItems = db.DrugItems.Where(d => d.ReferenceNumber == pvm.AppointmentReference).ToList();
            Doctor doctor = db.Doctors.Find(pvm.DoctorId);
            Patient patient = db.Patients.Find(pvm.PatientId);

            Prescription prescription = new Prescription();
            prescription.IssuedDate = DateTime.Today;
            prescription.Issuer = doctor;
            prescription.Patient = patient;
            prescription.Disease = pvm.Disease;
            prescription.Description = pvm.Description;
            prescription.DrugItems = DrugItems;
            prescription.AppointmentReference = pvm.AppointmentReference;
            prescription.IsPaid = false;
            prescription.IsCleared = false;

            db.Prescriptions.Add(prescription);
            db.SaveChanges();

            return RedirectToAction("SelectAppointment", "Appointment", new { appointmentId = pvm.AppointmentReference, role = "doctor", IsPrescriptionPresent = true });
        }


        //public ActionResult CreateDrugItem(int appointId, int drugId)
        //{
        //    Appointment appointment = db.Appointments.Find(appointId);
        //    AppointmentViewModel AppVM = new AppointmentViewModel();
        //    AppVM.ReferenceNumber = appointment.Id;
           

        //    return View();
        //}

        public ActionResult PayPrescriptionCharges()
        {
            List<Prescription> PrescriptionsInDb = db.Prescriptions.Include(p => p.Patient).Where(p => p.IsPaid == false)
                .Where(x => x.IsCleared == false).ToList();

            List<PrescriptionViewModel> PressVMList = new List<PrescriptionViewModel>();

            foreach(Prescription item in PrescriptionsInDb)
            {
                PrescriptionViewModel pressVM = new PrescriptionViewModel();
                pressVM.AppointmentReference = item.AppointmentReference;
                pressVM.PrescriptionId = item.Id;
                pressVM.AppointmentReference = item.AppointmentReference;

                pressVM.PatientName = item.Patient.FirstName + " " + item.Patient.Surname;
                PressVMList.Add(pressVM);
                ViewBag.Role = "cashier";
            }
            return View("PrescriptionList", PressVMList);
        }


        public ActionResult SelectPrescription(int prescriptionId, string role)
        {
            Prescription prescription = db.Prescriptions.Include(pa => pa.Patient).Where(p => p.Id == prescriptionId).FirstOrDefault();
            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.DrugItems = db.DrugItems.Include(d => d.Drug)
                .Where(p => p.ReferenceNumber == prescription.AppointmentReference).ToList();
            PresVM.AppointmentReference = prescription.AppointmentReference;
            PresVM.PatientName = prescription.Patient.FirstName + " " + prescription.Patient.Surname;
            PresVM.PrescriptionId = prescriptionId;
            PresVM.IsPaid = false;
            PresVM.IsCleared = false;
            double subTotal = 0;
            foreach(DrugItem item in PresVM.DrugItems)
            {
                subTotal += item.Total;
            }
            PresVM.SubTotal = subTotal;
            ViewBag.Role = role;
            return View("ViewPrescription", PresVM);
        }

        public ActionResult ConfirmPrescriptionCharge(int prescriptionId, double subTotal, PaymentType PaymentType)
        {
            if(PaymentType == 0)
            {
                PaymentType = PaymentType.Cash;
            }
            Prescription prescription = db.Prescriptions.Include(pa => pa.Patient).Where(p => p.Id == prescriptionId).FirstOrDefault();
            prescription.IsPaid = true;
            db.SaveChanges();

            Payment payment = new Payment();
            payment.Prescription = prescription;
            payment.PaymentDate = DateTime.Today;
            payment.PaymentType = PaymentType;
            payment.Amount = subTotal;

            string userId = User.Identity.GetUserId();
            payment.Cashier = db.StaffMembers.Find(userId);
            db.Payments.Add(payment);
            db.SaveChanges();

            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.DrugItems = db.DrugItems.Include(d => d.Drug)
                .Where(p => p.ReferenceNumber == prescription.AppointmentReference).ToList();
            PresVM.AppointmentReference = prescription.AppointmentReference;
            PresVM.PatientName = prescription.Patient.FirstName + " " + prescription.Patient.Surname;
            PresVM.PrescriptionId = prescriptionId;
            PresVM.IsPaid = true;
            PresVM.SubTotal = subTotal;
            ViewBag.Role = "cashier";

            return View("ViewPrescription", PresVM);
        }

        public ActionResult IssueDrugs()
        {
            List<Prescription> PrescriptionsInDb = db.Prescriptions.Include(p => p.Patient).Where(p => p.IsPaid == true)
               .Where(x => x.IsCleared == false).ToList();

            List<PrescriptionViewModel> PressVMList = new List<PrescriptionViewModel>();

            foreach (Prescription item in PrescriptionsInDb)
            {
                PrescriptionViewModel pressVM = new PrescriptionViewModel();
                pressVM.AppointmentReference = item.AppointmentReference;
                pressVM.PrescriptionId = item.Id;
                pressVM.AppointmentReference = item.AppointmentReference;

                pressVM.PatientName = item.Patient.FirstName + " " + item.Patient.Surname;
                PressVMList.Add(pressVM);
                ViewBag.Role = "pharmacist";
            }
            return View("PrescriptionList", PressVMList);
        }

        public ActionResult ClearPrescription(int prescriptionId, double subTotal)
        {
            Prescription prescription = db.Prescriptions.Include(pa => pa.Patient)
                .Where(p => p.Id == prescriptionId).FirstOrDefault();

            List<DrugItem> drugItems = db.DrugItems.Include(d => d.Drug)
           .Where(p => p.ReferenceNumber == prescription.AppointmentReference).ToList();

            string outOfStockMessage = null;

            foreach (DrugItem item in drugItems)
            {
                Drug drugInStock = db.Drugs.Find(item.Drug.Id);
                if(drugInStock.Quantity < item.OrderQuantity)
                {
                    outOfStockMessage = "There are only " + drugInStock.Quantity + " items of " + item.Drug.BrandName + " remaining in stock";
                    break;
                }
                else
                {
                    drugInStock.Quantity = drugInStock.Quantity - item.OrderQuantity;
                }
            }

            prescription.IsPaid = true;
            if (outOfStockMessage == null)
            {
                prescription.IsCleared = true;
                db.SaveChanges();
            }


            PrescriptionViewModel PresVM = new PrescriptionViewModel();
            PresVM.DrugItems = db.DrugItems.Include(d => d.Drug)
                .Where(p => p.ReferenceNumber == prescription.AppointmentReference).ToList();
            PresVM.AppointmentReference = prescription.AppointmentReference;
            PresVM.PatientName = prescription.Patient.FirstName + " " + prescription.Patient.Surname;
            PresVM.PrescriptionId = prescriptionId;
            PresVM.IsPaid = true;
            PresVM.IsCleared = prescription.IsCleared;
            PresVM.SubTotal = subTotal;
            ViewBag.Role = "pharmacist";
            ViewBag.StockMessage = outOfStockMessage;

            return View("ViewPrescription", PresVM);
        }

        public ActionResult AddFeedback()
        {
            string userId = User.Identity.GetUserId();
            Patient patient = db.Patients.Where(p => p.Id == userId).FirstOrDefault();
            List<Prescription> Prescriptions = db.Prescriptions.Where(p => p.Patient.Id == patient.Id)
                .Where(f => f.FeedbackSubmitted != true).ToList();

            return View("SelectPrescription", Prescriptions);
        }

        public ActionResult SubmitFeedback(int prescriptionId)
        {
            return View(db.Prescriptions.Find(prescriptionId));
        }

        public ActionResult SavePrescription(string Feedback, int prescriptionId)
        {
            Prescription prescriptionInDb = db.Prescriptions.Find(prescriptionId);
            prescriptionInDb.Feedback = Feedback;
            prescriptionInDb.FeedbackSubmitted = true;
            db.SaveChanges();

            return RedirectToAction("PatientMenu", "Menu", new { Message = "Feedback Submitted" });
        }




    }
}