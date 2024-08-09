using FasTagApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FasTagApi.Controllers
{
    public class ChangeController : ApiController
    {
        private readonly FasTagEntities1 _context = new FasTagEntities1(); // Replace with your actual DbContext class

        [HttpPost]
        public IHttpActionResult Post([FromBody] MobileChangeModel model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            if (string.IsNullOrWhiteSpace(model.VehicleNumber) ||
                string.IsNullOrWhiteSpace(model.OldMobileNumber) ||
                string.IsNullOrWhiteSpace(model.NewMobileNumber))

            {
                return BadRequest("Some required fields are missing or invalid.");
            }

            try
            {
                string Status = "Pending";
                string Remarks = "No";
                var mobileChange = new MobileChange
                {
                    VehicleNumber = model.VehicleNumber,
                    OldMobileNumber = model.OldMobileNumber,
                    NewMobileNumber = model.NewMobileNumber,
                    Status = Status,
                    Remarks = Remarks
                };

                _context.MobileChanges.Add(mobileChange);
                _context.SaveChanges();

                return Ok("Data inserted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var mobileChanges = _context.MobileChanges.ToList();
                if (mobileChanges == null || !mobileChanges.Any())
                {
                    return NotFound();
                }

                var response = mobileChanges.Select(mc => new
                {
                    VehicleNumber = mc.VehicleNumber,
                    OldMobileNumber = mc.OldMobileNumber,
                    NewMobileNumber = mc.NewMobileNumber,
                    Status = mc.Status,
                    Remarks = mc.Remarks
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}