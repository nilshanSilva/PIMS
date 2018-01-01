using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMS.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PIMS.Controllers;

namespace PIMS.Controllers
{
    public class FirstRun :Controller
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationUserManager _userManager;

  

        public async void SetUser()
        {
            StaffMember staffMember = new StaffMember
            {
                UserName = "nilshan@pims.com",
                FirstName = "Nilshan",
                Surname = "Silva",
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                ContactNumber = "1323467456",
                NIC = "1234567843v",
                UserLevel = UserLevel.IT_Manager,
                Email = "nilshan@pims.com",
                JoinedAt = DateTime.UtcNow
            };

            var result = await UserManager.CreateAsync(staffMember, "Nilshan@12");
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(staffMember.Id, staffMember.UserLevel.ToString());
            }

        }



}
}