using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMS.Models
{
    public class Actions
    {
        public int Id { get; set; }
        public Drug Drug { get; set; }
        public StaffMember Pharmacist { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public ActionType ActionType { get; set; }
        public int Balance { get; set; }
        public int ActionedQuantity { get; set; }
        public int ActionedDate { get; set; }
    }

    public enum ActionType {Add = 1, Update, Issue}
}