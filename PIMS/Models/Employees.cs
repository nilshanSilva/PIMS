using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models
{
    public enum UserLevel { Cashier = 1, Pharmacist, Medical_Superintendent, IT_Manager }
    public enum Gender { Male = 1, Female }
    public enum Specialization { Audiologist = 1, Cardiologist, Dermatologist, Neurologist, Ophthalmologist}
    public enum Qualification { MBBS = 1}
    public enum City { Bristol = 1, Belfast, Cambridge, Derby, Leeds, Liverpool, London, Manchester, Oxford, Westminister, Glasgow}
    public enum Room { R1 = 1, R2, R3, R4, R5, R6}

    public class Doctor : ApplicationUser
    {
        public Specialization Specialization { get; set; }
        public Qualification Qualification { get; set; }
        public double ChannelFee { get; set; }

        [Display(Name = "Channel Start Time"), DataType(DataType.Time), Required]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ChannelStartTime { get; set; }

        public int AverageChannelDuration { get; set; }
        public DateTime ChannelEndTime { get; set; }
        public int NumOfPatientsPerDay { get; set; }
        public Room Room { get; set; }
    }

    public class Patient : ApplicationUser
    {
        public City City { get; set; }
        public string Address { get; set; }
    }

    public class StaffMember : ApplicationUser
    {
        public UserLevel UserLevel { get; set; }
    }



}