using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace PIMS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }

        [Display(Name = "Birth Date"), DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required, Display(Name = "Contact Number")]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string ContactNumber { get; set; }

        [Required, Display(Name = "NIC Card Number")]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string NIC { get; set; }

        [ScaffoldColumn(false)]
        public DateTime JoinedAt { get; set; }
    }

    public class RegisterStaffMemberViewModel : RegisterViewModel
    {
        [Required]
        public UserLevel UserLevel { get; set; }
    }

    public class RegisterDoctorViewModel : RegisterViewModel
    {
        [Required]
        public Specialization Specialization { get; set; }

        [Required]
        public Qualification Qualification { get; set; }

        [Required, Display(Name = "Channel Fee ($)")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Number")]
        [Range(1, 300, ErrorMessage = "The fee must be reasonable")]
        public double ChannelFee { get; set; }

        [Display(Name = "Channel Start Time"), DataType(DataType.Time), Required]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ChannelStartTime { get; set; }

        [Required, Display(Name = "Average Channel Duration (Mins)")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 31, ErrorMessage = "The time must fit in a reasonable range")]
        public int AverageChannelDuration { get; set; }

        [Display(Name = "Channel End Time"), DataType(DataType.Time), Required]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ChannelEndTime { get; set; }

        [Display(Name ="Number of Patients/Day")]
        public int NumOfPatientsPerDay { get; set; }

        [Required]
        public Room Room { get; set; }
    }

    public class RegisterPatientViewModel : RegisterViewModel
    {
        [Required]
        public City City { get; set; }

        [Required]
        public string Address { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
