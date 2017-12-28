using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Threading.Tasks;

namespace PIMS.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public MenuController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        [HttpGet, Authorize]
        public ActionResult GetDashboard()
        {
            string userId = User.Identity.GetUserId();
            Patient patient = db.Users.OfType<Patient>().Where(x => x.Id == userId).FirstOrDefault();
            if (patient != null)
                return RedirectToAction("Index", "Home", null);

            Doctor doctor = db.Users.OfType<Doctor>().Where(x => x.Id == userId).FirstOrDefault();
            if (doctor != null)
                return RedirectToAction("DoctorMenu");

            StaffMember staffMember = db.Users.OfType<StaffMember>().Where(x => x.Id == userId).FirstOrDefault();
            if(staffMember != null)
            {
                   if (staffMember.UserLevel == UserLevel.IT_Manager)
                    return RedirectToAction("AdminMenu");
                
                   else if(staffMember.UserLevel == UserLevel.Cashier)
                    return RedirectToAction("CashierMenu");

                   else if(staffMember.UserLevel == UserLevel.Medical_Superintendent)
                    return RedirectToAction("AdminMenu"); //change this

                   else if(staffMember.UserLevel == UserLevel.Pharmacist)
                    return RedirectToAction("PharmacistMenu");
            }

                return View();
        }

        [HttpGet]
        public ActionResult AdminMenu(string message)
        {
            if (message != null)
            {
                ViewBag.Message = message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult PatientMenu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DoctorMenu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashierMenu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PharmacistMenu()
        {
            return View();
        }



    }
}