using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PIMS.Models
{
    public class Drug
    {
        public int Id { get; set; }

        [Required, Display(Name = "Generic Name")]
        public string GenericName { get; set; }

        [Required, Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        public string Description { get; set; }

        [Required]
        public DrugCategory Category { get; set; }

        [Required, Display(Name = "Net Content")]
        public string NetContent { get; set; }

        [Required, Display(Name = "Form")]
        public DrugForm DrugForm { get; set; }

        [Required, Display(Name = "Recieved Price")]
        public double RecievedPrice { get; set; }

        [Required, Display(Name = "Retail Price")]
        public double RetailPrice { get; set; }

        public int Quantity { get; set; }

        [Required, Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required, Display(Name = "Re-Order Level")]
        public int ReOrderLevel { get; set; }

        [Required]
        public Shelf Shelf { get; set; }
    }

    public enum DrugForm { Pills = 1, Capsule, Powder, Liquid, Syrup, Ointment}
    public enum DrugCategory { Food_and_Beverage = 1, Cosmetics, Suppliments }
    public enum Shelf { SA = 1, SB, SC, SD}
}