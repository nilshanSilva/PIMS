using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PIMS.Models;

namespace PIMS.Controllers
{
    public class InventoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inventory
        public ActionResult Index(string context)
        {
            ViewBag.Context = context;
            return View(db.Drugs.ToList());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drug drug = db.Drugs.Find(id);
            if (drug == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = drug.Category.ToString().Replace("_", " ");
            return View(drug);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GenericName,BrandName,Description,Category,NetContent,DrugForm,RecievedPrice,RetailPrice,Quantity,SupplierName,ReOrderLevel,Shelf")] Drug drug)
        {
            if (ModelState.IsValid)
            {
                db.Drugs.Add(drug);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drug);
        }

        // GET: Inventory/Edit/5
        public ActionResult UpdateStock(int? id, string context)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drug drug = db.Drugs.Find(id);
            if (drug == null)
            {
                return HttpNotFound();
            }

            if(context != null)
            {
                ViewBag.Context = context;
            }
            return View(drug);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStock( int drugId, Drug drug, string ViewBagContext)
        {
                Drug drugInDb = db.Drugs.Find(drugId);
                drugInDb.Quantity = drug.Quantity;
                db.SaveChanges();
            if(ViewBagContext != null)
            {
                ViewBag.Context = ViewBagContext;
            }
            return View(drugInDb);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
