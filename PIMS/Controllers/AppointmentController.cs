using PIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMS.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PIMS.Controllers
{
    public class AppointmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public AppointmentController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Appointment
        public ActionResult AppointmentMenu([Bind(Include = "PatientId,IsConfirmed,IsDateSubmitted")] AppointmentViewModel appointmentDetails)
        {
            AppointmentViewModel appointment = new AppointmentViewModel();
            appointment.IsDateSubmitted = false;
            appointment.IsConfirmed = false;

            if (appointmentDetails.PatientId == null)
            {
                    string userId = User.Identity.GetUserId();
                    Patient patient = db.Users.OfType<Patient>().Where(p => p.Id == userId).FirstOrDefault();

                    if (patient == null)
                    {
                        List<Patient> patients = db.Patients.Take(5).ToList();
                        appointment.Patients = patients;
                        return View(appointment);
                    }
                    else
                    {
                        appointment.PatientId = patient.Id;
                        return RedirectToAction("AppointmentMenu", appointment);
                    }              
            }
            else
            {
                Patient patient = db.Patients.Find(appointmentDetails.PatientId);
                appointmentDetails.Patient = patient;
                List<Doctor> doctors = db.Doctors.Take(5).ToList();
                appointmentDetails.Doctors = doctors;
               
                return View(appointmentDetails);
            }
        }



        public ActionResult SearchDoctor(AppointmentViewModel appointment, string name)
        {
            if(name != null)
            {
                var doctorsByFirstName = db.Doctors.Where(n => n.FirstName.Contains(name)).Take(5).ToList();
                if (doctorsByFirstName != null)
                {
                    appointment.Doctors = doctorsByFirstName;
                    return View("_SearchMember", appointment);
                }

                var doctorsBySurname = db.Doctors.Where(n => n.Surname.Contains(name)).Take(5).ToList();
                if (doctorsBySurname != null)
                {
                    appointment.Doctors = doctorsBySurname;
                    return View("_SearchMember", appointment);
                }

                return View("_SearchMember", appointment);
            }
            //else

            if(appointment.Doctors != null)
            {
                return View("_SearchMember", appointment);
            }
            else
            {
                appointment.Doctors = db.Doctors.Take(5).ToList();
                return View("_SearchMember", appointment);
            }
            
        }

        public ActionResult SearchPatient(AppointmentViewModel appointment, string name)
        {
            if (name != null)
            {
                var patientByFirstName = db.Patients.Where(n => n.FirstName.Contains(name)).Take(5).ToList();
                if (patientByFirstName != null)
                {
                    appointment.Patients = patientByFirstName;
                    return View("_SearchMember", appointment);
                }

                var patientsBySurname = db.Patients.Where(n => n.Surname.Contains(name)).Take(5).ToList();
                if (patientsBySurname != null)
                {
                    appointment.Patients = patientsBySurname;
                    return View("_SearchMember", appointment);
                }

                return View("_SearchMember", appointment);
            }
            //else

            if (appointment.Patients != null)
            {
                return View("_SearchMember", appointment);
            }
            else
            {
                appointment.Patients = db.Patients.Take(5).ToList();
                return View("_SearchMember", appointment);
            }
        }

        public ActionResult SelectDoctor(string patientId, string doctorId)
        {
            AppointmentViewModel appt = new AppointmentViewModel();
            appt.Doctor = db.Doctors.Find(doctorId);
            appt.Patient = db.Patients.Find(patientId);
            appt.IsDateSubmitted = false;
            appt.IsConfirmed = false;
            return View("AppointmentMenu", appt);
       }

        public ActionResult SelectPatient(string patientId)
        {
            AppointmentViewModel appt = new AppointmentViewModel();
            appt.Patient = db.Patients.Find(patientId);
            appt.IsDateSubmitted = false;
            appt.IsConfirmed = false;
            appt.PatientId = patientId;
            return RedirectToAction("AppointmentMenu", appt);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAppointmentDetails([Bind(Include = "ChannelDate,Patient,ChannelFee,Doctor,PatientId, DoctorId") ] AppointmentViewModel appointmentDetails)
        {
            appointmentDetails.Doctor = db.Doctors.Find(appointmentDetails.DoctorId);
            appointmentDetails.Patient = db.Patients.Find(appointmentDetails.PatientId);
            appointmentDetails.IsDateSubmitted = true;
            appointmentDetails.IsConfirmed = false;
            string Room = appointmentDetails.Doctor.Room.ToString() + "-";

            List<Appointment> channelsForTheDay = db.Appointments.Where(d => d.DoctorId == appointmentDetails.DoctorId)
                .Where(t => t.ChannelDate == appointmentDetails.ChannelDate).ToList();

            if (channelsForTheDay == null || channelsForTheDay.Count < 1)
            {
                appointmentDetails.ChannelDate = appointmentDetails.ChannelDate
                    .AddHours(appointmentDetails.Doctor.ChannelStartTime.Hour)
                    .AddMinutes(appointmentDetails.Doctor.ChannelStartTime.Minute);

                appointmentDetails.ChannelNumber = Room + "1";
                appointmentDetails.ChannelNum = 1;
                ViewBag.AppointmentMessage = "Available";
            }
            else
            {
                if(channelsForTheDay.Count < appointmentDetails.Doctor.NumOfPatientsPerDay)
                {
                    TimeSpan channelDuration = TimeSpan.FromMinutes(appointmentDetails.Doctor.AverageChannelDuration);
                    TimeSpan channelSpan = TimeSpan.FromTicks(channelDuration.Ticks * channelsForTheDay.Count);
                    appointmentDetails.ChannelDate = appointmentDetails.ChannelDate
                        .AddHours(appointmentDetails.Doctor.ChannelStartTime.Hour)
                        .AddMinutes(appointmentDetails.Doctor.ChannelStartTime.Minute) + channelSpan;

                    appointmentDetails.ChannelNumber = Room + (channelsForTheDay.Count + 1).ToString();
                    appointmentDetails.ChannelNum = channelsForTheDay.Count + 1;
                    ViewBag.AppointmentMessage = "Available";
                }
                else //look if the next day could be reserved
                {
                    appointmentDetails.ChannelDate = appointmentDetails.ChannelDate.AddDays(1);
                    List<Appointment> channelsForTheSecondDay = db.Appointments.Where(d => d.DoctorId == appointmentDetails.DoctorId)
                    .Where(t => t.ChannelDate == appointmentDetails.ChannelDate).ToList();

                    if (channelsForTheSecondDay == null || channelsForTheSecondDay.Count < 1)
                    {
                        appointmentDetails.ChannelDate = appointmentDetails.ChannelDate
                            .AddHours(appointmentDetails.Doctor.ChannelStartTime.Hour)
                    .AddMinutes(appointmentDetails.Doctor.ChannelStartTime.Minute);

                        appointmentDetails.ChannelNumber = Room + "1";
                        appointmentDetails.ChannelNum = 1;
                        ViewBag.AppointmentMessage = "Next-Day-Available";
                    }
                    else
                    {
                        if (channelsForTheSecondDay.Count < appointmentDetails.Doctor.NumOfPatientsPerDay)
                        {
                            TimeSpan channelDuration = TimeSpan.FromMinutes(appointmentDetails.Doctor.AverageChannelDuration);
                            TimeSpan channelSpan = TimeSpan.FromTicks(channelDuration.Ticks * channelsForTheSecondDay.Count);
                            appointmentDetails.ChannelDate = appointmentDetails.ChannelDate.AddHours(appointmentDetails.Doctor.ChannelStartTime.Hour)
                            .AddMinutes(appointmentDetails.Doctor.ChannelStartTime.Minute) + channelSpan;

                            appointmentDetails.ChannelNumber = Room + (channelsForTheSecondDay.Count + 1).ToString();
                            appointmentDetails.ChannelNum = channelsForTheSecondDay.Count + 1;
                            ViewBag.AppointmentMessage = "Next-Day-Available";
                        }
                        else
                        {
                            //The next day is reserved too
                            ViewBag.AppointmentMessage = "Unavailable";
                        }
                    }
                }
            }
            appointmentDetails.ChannelTime = appointmentDetails.ChannelDate;
            

            return View("AppointmentMenu", appointmentDetails);
        }

        public ActionResult ConfirmAppointment([Bind(Include = "PatientId, DoctorId, ChannelDate, ChannelTime, ChannelNumber," +
            "ChannelNum, IsDateSubmitted, IsConfirmed, ViewBagMessage")] AppointmentViewModel appointmentDetails)
        {
            appointmentDetails.Doctor = db.Doctors.Find(appointmentDetails.DoctorId);
            appointmentDetails.Patient = db.Patients.Find(appointmentDetails.PatientId);
            ViewBag.AppointmentMessage = appointmentDetails.ViewBagMessage;
            DateTime chnlDate = appointmentDetails.ChannelTime.Date;

            Appointment appointment = new Appointment();
            appointment.ChannelDate = chnlDate;
            appointment.CreatedDate = DateTime.Today;
            appointment.PatientId = appointmentDetails.PatientId;
            appointment.DoctorId = appointmentDetails.DoctorId;
            appointment.ChannelNumber = appointmentDetails.ChannelNum;
            appointment.ChannelFee = appointmentDetails.Doctor.ChannelFee;
            appointment.IsPaid = false;
            appointment.IsChanneled = false;

            db.Appointments.Add(appointment);
            db.SaveChanges();

            appointmentDetails.ReferenceNumber = appointment.Id;
            appointmentDetails.IsConfirmed = true;
     
            return View("AppointmentMenu", appointmentDetails);
        }

        

        public ActionResult UpdateAppointment([Bind(Include = "ReferenceNumber,PatientId, DoctorId, ChannelDate, ChannelTime, ChannelNumber," +
            "ChannelFee,ViewBagMessage, IsPaid, IsChanneled, IsPrescriptionGenerated")] AppointmentViewModel appointmentDetails)
        {
            appointmentDetails.Doctor = db.Doctors.Find(appointmentDetails.DoctorId);
            appointmentDetails.Patient = db.Patients.Find(appointmentDetails.PatientId);

            Appointment appointmentInDb = db.Appointments.Find(appointmentDetails.ReferenceNumber);
            appointmentInDb.IsPaid = appointmentDetails.IsPaid;
            appointmentInDb.IsChanneled = appointmentDetails.IsChanneled;
            db.SaveChanges();

            ViewBag.Role = appointmentDetails.ViewBagMessage;
            return View("ViewAppointment", appointmentDetails);
        }

        public ActionResult MarkChannel()
        {
            List<Appointment> appointmentsInDb = db.Appointments.Where(a => a.IsPaid == true)
                .Where(c => c.IsChanneled == false).Where(d => d.ChannelDate == DateTime.Today).ToList();

            List<AppointmentViewModel> AppVMList = new List<AppointmentViewModel>();
            foreach (Appointment item in appointmentsInDb)
            {
                AppointmentViewModel appVM = new AppointmentViewModel();
                appVM.Patient = db.Patients.Find(item.PatientId);
                appVM.Doctor = db.Doctors.Find(item.DoctorId);
                appVM.ChannelNumber = appVM.Doctor.Room.ToString() + "-" + item.ChannelNumber.ToString();
                appVM.ReferenceNumber = item.Id;
                appVM.IsChanneled = item.IsChanneled;
                AppVMList.Add(appVM);
                ViewBag.Role = "doctor";
            }
            return View("AppointmentList", AppVMList);
        }

        public ActionResult PayAppointmentBill() //Loads the View
        {
            List<Appointment> appointmentsInDb = db.Appointments.Where(a => a.IsPaid == false)
                  .Where(d => d.CreatedDate == DateTime.Today).ToList();

            List<AppointmentViewModel> AppVMList = new List<AppointmentViewModel>();

            foreach(Appointment item in appointmentsInDb)
            {
                AppointmentViewModel appVM = new AppointmentViewModel();
                appVM.Patient = db.Patients.Find(item.PatientId);
                Doctor doc = db.Doctors.Find(item.DoctorId);
                appVM.ChannelNumber = doc.Room.ToString() + "-" + item.ChannelNumber.ToString();
                appVM.ReferenceNumber = item.Id;
                AppVMList.Add(appVM);
                ViewBag.Role = "cashier";
            }
            return View("AppointmentList", AppVMList);
        }

        public ActionResult SearchAppointment(int? referenceNumber)
        {
            if(referenceNumber != null)
            {
                Appointment appointment = db.Appointments.Find(referenceNumber);

                List<AppointmentViewModel> AppVMList = new List<AppointmentViewModel>();

                AppointmentViewModel appVM = new AppointmentViewModel();
                appVM.Patient = db.Patients.Find(appointment.PatientId);
                Doctor doc = db.Doctors.Find(appointment.DoctorId);
                appVM.ChannelNumber = doc.Room.ToString() + "-" + appointment.ChannelNumber.ToString();
                appVM.ReferenceNumber = appointment.Id;
                AppVMList.Add(appVM);

                return View("AppointmentList", AppVMList);
            }
            else
            {
                List<Appointment> appointmentsInDb = db.Appointments.Where(a => a.IsPaid == false)
                    .Where(d => d.CreatedDate == DateTime.Today).ToList();

                List<AppointmentViewModel> AppVMList = new List<AppointmentViewModel>();

                foreach (Appointment item in appointmentsInDb)
                {
                    AppointmentViewModel appVM = new AppointmentViewModel();
                    appVM.Patient = db.Patients.Find(item.PatientId);
                    Doctor doc = db.Doctors.Find(item.DoctorId);
                    appVM.ChannelNumber = doc.Room.ToString() + "-" + item.ChannelNumber.ToString();
                    appVM.ReferenceNumber = item.Id;
                    AppVMList.Add(appVM);
                }

                return View("AppointmentList", AppVMList);
            }
        }

        public ActionResult SelectAppointment(int appointmentId, string role, bool? IsPrescriptionPresent)
        {
            Appointment appointment = db.Appointments.Find(appointmentId);
            List<Drug> DrugList = db.Drugs.Take(5).ToList();

            AppointmentViewModel appVM = new AppointmentViewModel();
            appVM.Patient = db.Patients.Find(appointment.PatientId);
            appVM.Doctor = db.Doctors.Find(appointment.DoctorId);
            appVM.ChannelNumber = appVM.Doctor.Room.ToString() + "-" + appointment.ChannelNumber.ToString();
            appVM.ReferenceNumber = appointment.Id;
            appVM.ChannelDate = appointment.ChannelDate;
            appVM.Drugs = DrugList;
            ViewBag.Role = role;
            if(IsPrescriptionPresent != null)
            {
                appVM.IsPrescriptionGenerated = (bool) IsPrescriptionPresent;
            }
            return View("ViewAppointment", appVM);
        }


    }
}