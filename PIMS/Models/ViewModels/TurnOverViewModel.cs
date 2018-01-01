using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMS.Models.ViewModels
{
    public class PeriodicDataSet
    {
        public DateTime Date { get; set; }
        public double TurnOver { get; set; }
    }

    public class TurnOverViewModel
    {
        public List<PeriodicDataSet> DataList { get; set; }

    }
}