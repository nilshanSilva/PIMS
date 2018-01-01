using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PIMS.Models.ViewModels;

namespace PIMS.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public ReportController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult InventoryReport()
        {
            return View(db.Drugs.ToList());
        }

        public ActionResult FinancialReport(string category)
        {
            if(category == null)
            {
                category = "doctor";
            }

            if(category == "doctor")
            {
                List<Prescription> PrescriptionsList = db.Prescriptions.ToList();
            }

            return View();
        }

        //public ActionResult MonthlySalesReport(string category)
        //{
        //    if(category == null)
        //    {
        //        category = "drugs";
        //    }

        //    if(category == "drugs")
        //    {
        //        List<Drug> Drugs = new List<Drug>();
        //        List<Prescription> Prescriptions = db.Prescriptions.Include(d => d.DrugItems).Where(t => t.IssuedDate.Month == DateTime.Today.Month).ToList();
        //        foreach(Prescription item in Prescriptions)
        //        {
        //            foreach(DrugItem drugitem in item.DrugItems)
        //            {
        //                if (Drugs.Contains(drugitem.Drug))
        //            }

        //        }
        //    }

        //    return View();
        //}

        public ActionResult YearlyTurnOverReport()
        {
            //Dummy Data
            List<PeriodicDataSet> DummyList = new List<PeriodicDataSet>();
            DateTime currenyYear = DateTime.Now.AddYears(-5);
            double firstYearProfit = 6000;
            for (int i = 0; i<5; i++)
            {
                PeriodicDataSet dummyset = new PeriodicDataSet();
                dummyset.Date = currenyYear.AddYears(i);
                dummyset.TurnOver = firstYearProfit + (i * 100);
                DummyList.Add(dummyset);
            }
            TurnOverViewModel TOVM = new TurnOverViewModel();
            TOVM.DataList = DummyList;
            return View(TOVM);
        }

        public ActionResult PatientReport()
        {
            return View(db.Patients.ToList());
        }

        public ActionResult DoctorReport(/*string specialization = null*/)
        {
            List<RegisterDoctorViewModel> DoctorVMList = new List<RegisterDoctorViewModel>();
            //var query = db.Doctors.AsEnumerable();
            List<Doctor> DoctorsInDb = db.Doctors.ToList();
            foreach(Doctor item in DoctorsInDb)
            {
                RegisterDoctorViewModel DoctorVM = new RegisterDoctorViewModel();
                DoctorVM.FirstName = item.FirstName;
                DoctorVM.Surname = item.Surname;
                DoctorVM.BirthDate = item.BirthDate;
                DoctorVM.Gender = item.Gender;
                DoctorVM.ContactNumber = item.ContactNumber;
                DoctorVM.NIC = item.NIC;
                DoctorVM.Specialization = item.Specialization;
                DoctorVM.Qualification = item.Qualification;
                DoctorVM.ChannelFee = item.ChannelFee;
                DoctorVM.ChannelStartTime = item.ChannelStartTime;
                DoctorVM.ChannelEndTime = item.ChannelEndTime;
                DoctorVM.AverageChannelDuration = item.AverageChannelDuration;
                DoctorVM.NumOfPatientsPerDay = item.NumOfPatientsPerDay;
                DoctorVM.Room = item.Room;

                DoctorVMList.Add(DoctorVM);
            }
            return View(DoctorVMList);
        }


    }
}