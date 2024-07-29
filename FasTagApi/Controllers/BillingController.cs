using FasTagApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data;
using OfficeOpenXml;

namespace FasTagApi.Controllers
{
    public class BillingController : Controller
    {
        private readonly FasTagEntities1 _context = new FasTagEntities1(); // Replace with your actual DbContext class

        // GET: Billing
        public ActionResult Index()
        {
            try
            {
                return View(_context.Billings.ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        // GET: Billing/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var billing = _context.Billings.Find(id);
                if (billing == null)
                {
                    return HttpNotFound();
                }
                return View(billing);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        // GET: Billing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Billing/Create
        [HttpPost]
        public ActionResult Create(Billing billing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Billings.Add(billing);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Bill details saved successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var validationErrors in e.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    return new HttpStatusCodeResult(500, "Internal Server Error");
                }
            }
            return View(billing);
        }

        // GET: Billing/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var billing = _context.Billings.Find(id);
                if (billing == null)
                {
                    return HttpNotFound();
                }
                return View(billing);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        // POST: Billing/Edit/5
        [HttpPost]
        public ActionResult Edit(Billing billing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(billing).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Bill details updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var validationErrors in e.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    return new HttpStatusCodeResult(500, "Internal Server Error");
                }
            }
            return View(billing);
        }

        // GET: Billing/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var billing = _context.Billings.Find(id);
                if (billing == null)
                {
                    return HttpNotFound();
                }
                return View(billing);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        // POST: Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var billing = _context.Billings.Find(id);
                _context.Billings.Remove(billing);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Bill details deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (excelFile != null && excelFile.ContentLength > 0)
            {
                try
                {
                    using (var package = new ExcelPackage(excelFile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var newBilling = new Billing
                            {
                                BillNo = worksheet.Cells[row, 1].Text,
                                BillDate = DateTime.Parse(worksheet.Cells[row, 2].Text),
                                Amount = decimal.Parse(worksheet.Cells[row, 3].Text),
                                DeptCode = worksheet.Cells[row, 4].Text,
                                Department = worksheet.Cells[row, 5].Text,
                                PartyCode = worksheet.Cells[row, 6].Text,
                                PartyName = worksheet.Cells[row, 7].Text,
                                Mode = worksheet.Cells[row, 8].Text,
                                RefNo = worksheet.Cells[row, 9].Text,
                                DateOfPayment = DateTime.Parse(worksheet.Cells[row, 10].Text),
                            };
                            _context.Billings.Add(newBilling);
                        }
                        _context.SaveChanges();
                    }
                    TempData["SuccessMessage"] = "Bill Details saved Successfully from Excel";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    return new HttpStatusCodeResult(500, "Internal Server Error");
                }
            }
            TempData["ErrorMessage"] = "Please upload a valid Excel file.";
            return RedirectToAction("Create");
        }

        public JsonResult GetDepartmentByCode(int deptCode)
        {
            var department = _context.DMMASTERs
                                    .Where(d => d.SNO == deptCode)
                                    .Select(d => new { Department = d.Name })
                                    .FirstOrDefault();
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartyByCode(int partyCode)
        {
            var party = _context.PMMASTERs
                               .Where(p => p.SNO == partyCode)
                               .Select(p => new { PartyName = p.PARTYNAME })
                               .FirstOrDefault();
            return Json(party, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] selectedSnos)
        {
            if (selectedSnos != null && selectedSnos.Any())
            {
                foreach (var sno in selectedSnos)
                {
                    var billing = _context.Billings.Find(sno);
                    if (billing != null)
                    {
                        _context.Billings.Remove(billing);
                    }
                }
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Selected records have been deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No records were selected for deletion.";
            }

            return RedirectToAction("Index");
        }
    }
}