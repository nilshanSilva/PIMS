using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models.ViewModels
{
    public class SalesViewModel
    {
        public Drug Drug { get; set; }
        public Doctor Doctor { get; set; }

        [Display(Name ="Items Sold")]
        public int ItemsSold { get; set; }

        public double Expenses { get; set; }
        public double Earnings { get; set; }

        [Display(Name ="Total Profit")]
        public double TotalProfit { get; set; }
    }
}