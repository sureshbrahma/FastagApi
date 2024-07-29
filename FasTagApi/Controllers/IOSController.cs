using FasTagApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FasTagApi.Controllers
{
    public class IOSController : Controller
    {
        private FasTagEntities1 db = new FasTagEntities1();

        // GET: IOS
        public ActionResult Index()
        {
            return View();
        }

        // GET: FastagForm
        public ActionResult FastagForm()
        {
            return View(new VehicleRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FastagForm(VehicleRequest vehicleRequest)
        {
            if (ModelState.IsValid)
            {
                vehicleRequest.Status = "Pending"; // Set initial status
                vehicleRequest.Expiration = "NO";
                vehicleRequest.ReferenceNo = GenerateReferenceNumber(); // Generate reference number

                db.VehicleRequests.Add(vehicleRequest);
                db.SaveChanges();

                // Return success JSON response with reference number
                return Json(new { success = true, referenceNo = vehicleRequest.ReferenceNo });
            }

            // If ModelState is not valid, return error response or handle accordingly
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        private string GenerateReferenceNumber()
        {
            var now = DateTime.Now;
            return $"REF-{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}";
        }

        // GET: VehicleRequest/CheckStatus
        public ActionResult CheckStatus()
        {
            return View();
        }

        // POST: VehicleRequest/GetReferenceNumbers
        [HttpPost]
        public JsonResult GetReferenceNumbers(string vehicleNumber)
        {
            if (string.IsNullOrWhiteSpace(vehicleNumber))
            {
                return Json(new { success = false, message = "Vehicle number is required" }, JsonRequestBehavior.AllowGet);
            }

            var normalizedVehicleNumber = vehicleNumber.Trim().ToLower();

            var referenceNumbers = db.VehicleRequests
          .Where(v => v.VehicleFullNumber.ToLower() == normalizedVehicleNumber && v.Expiration.ToLower() == "no")
          .Select(v => v.ReferenceNo)
          .ToList();

            if (!referenceNumbers.Any())
            {
                return Json(new { success = false, message = "No active reference numbers found for the vehicle number" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, referenceNumbers }, JsonRequestBehavior.AllowGet);
        }

        // POST: VehicleRequest/GetStatus
        [HttpPost]
        public JsonResult GetStatus(string referenceNumber)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber))
            {
                return Json(new { success = false, message = "Reference number is required" }, JsonRequestBehavior.AllowGet);
            }

            var request = db.VehicleRequests.FirstOrDefault(v => v.ReferenceNo == referenceNumber);

            if (request == null)
            {
                return Json(new { success = false, message = "Reference number not found" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, status = request.Status }, JsonRequestBehavior.AllowGet);
        }

        // GET: MobileChange
        public ActionResult IndexMobile()
        {
            return View();
        }

        // POST: MobileChange/Submit
        [HttpPost]
        public ActionResult Submit(MobileChange model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = "Pending";
                    model.Remarks = "NO";
                    db.MobileChanges.Add(model);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Your mobile number change request has been submitted successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error: {ex.Message}" });
                }
            }

            return Json(new { success = false, message = "Invalid data." });
        }

        [HttpGet]
        public ActionResult IndexStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FetchStatus(string vehicleNumber, string oldMobileNumber, string newMobileNumber)
        {
            var results = db.MobileChanges
                .Where(mc => mc.VehicleNumber.ToLower().Trim() == vehicleNumber.ToLower().Trim() &&
                             mc.OldMobileNumber.ToLower().Trim() == oldMobileNumber.ToLower().Trim() &&
                             mc.NewMobileNumber.ToLower().Trim() == newMobileNumber.ToLower().Trim())
                .ToList();

            if (!results.Any())
            {
                ViewBag.ErrorMessage = "No request found for the entered vehicle number, old mobile number, and new mobile number";
            }

            return View("IndexStatus", results);
        }
    }
}