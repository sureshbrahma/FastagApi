using FasTagApi.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace FasTagApi.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly FasTagEntities1 _context = new FasTagEntities1(); // Replace with your actual DbContext class

        // GET api/payment
        [HttpGet]
        [Route("api/payment")]
        public IHttpActionResult Get()
        {
            try
            {
                var vehicleRequests = _context.Billings.ToList();
                if (vehicleRequests == null || !vehicleRequests.Any())
                {
                    return NotFound();
                }

                var response = vehicleRequests.Select(vr => new
                {
                    BillNo = vr.BillNo,
                    BillDate = vr.BillDate.ToString("yyyy-MM-dd"),
                    Amount = vr.Amount,
                    Department = vr.Department,
                    PartyName = vr.PartyName,
                    Mode = vr.Mode,
                    RefNo = vr.RefNo,
                    DateOfPayment = vr.DateOfPayment?.ToString("yyyy-MM-dd"),
                    ViewStatus = vr.ViewStatus,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/payment/update")]
        public IHttpActionResult UpdateViewStatus([FromBody] dynamic data)
        {
            try
            {
                string billNo = data.billNo;
                string role = data.role;
                var billing = _context.Billings.FirstOrDefault(b => b.BillNo == billNo);
                if (billing == null)
                {
                    return NotFound();
                }

                switch (billing.ViewStatus)
                {
                    case null:
                    case "":
                        billing.ViewStatus = role == "Seller" ? "viewed by seller" : "viewed by department";
                        break;

                    case "viewed by seller":
                        if (role == "Department")
                        {
                            billing.ViewStatus = "viewed by both";
                        }
                        break;

                    case "viewed by department":
                        if (role == "Seller")
                        {
                            billing.ViewStatus = "viewed by both";
                        }
                        break;
                }

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
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