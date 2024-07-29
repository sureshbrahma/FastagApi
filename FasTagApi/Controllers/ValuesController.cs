using System;
using System.Linq;
using System.Web.Http;
using FasTagApi.Models; // Replace with your actual namespace

namespace FasTagApi.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly FasTagEntities1 _context = new FasTagEntities1(); // Replace with your actual DbContext class

        // POST api/values
        [HttpPost]
        public IHttpActionResult Post([FromBody] VehicleRequestModel model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            // Additional validation can be added here if needed
            if (string.IsNullOrWhiteSpace(model.InstitutionName) ||
                string.IsNullOrWhiteSpace(model.DepartmentName) ||
                string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.WhatsappMobileNumber) ||
                string.IsNullOrWhiteSpace(model.VehicleFullNumber) ||
                string.IsNullOrWhiteSpace(model.VehicleType) ||
                string.IsNullOrWhiteSpace(model.TravellingFromTo) ||
                string.IsNullOrWhiteSpace(model.DepartmentInChargePermission) ||
                model.RechargeAmount <= 0 ||  // Check for positive RechargeAmount
                model.DateOfRequest == default ||
                string.IsNullOrWhiteSpace(model.ReferenceNumber))
            {
                return BadRequest("Some required fields are missing or invalid.");
            }

            try
            {
                string status = "Pending";
                string expiration = "NO";
                string fastagAccount = string.Empty;
                string remarks = string.Empty;

                var vehicleRequest = new VehicleRequest
                {
                    InstitutionName = model.InstitutionName,
                    DepartmentName = model.DepartmentName,
                    UserName = model.UserName,
                    WhatsappMobileNumber = model.WhatsappMobileNumber,
                    VehicleFullNumber = model.VehicleFullNumber,
                    VehicleType = model.VehicleType,
                    TravellingFromTO = model.TravellingFromTo,
                    DepartmentInChargePermission = model.DepartmentInChargePermission,
                    RechargeAmount = model.RechargeAmount,
                    DateOfRequest = model.DateOfRequest,
                    ReferenceNo = model.ReferenceNumber,
                    Status = status,
                    Expiration = expiration,
                    FastagAccount = fastagAccount,
                    Remarks = remarks
                };

                _context.VehicleRequests.Add(vehicleRequest);
                _context.SaveChanges();

                return Ok("Data inserted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return InternalServerError(ex);
            }
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var vehicleRequests = _context.VehicleRequests.ToList();
                if (vehicleRequests == null || !vehicleRequests.Any())
                {
                    return NotFound();
                }

                var response = vehicleRequests.Select(vr => new
                {
                    InstitutionName = vr.InstitutionName,
                    DepartmentName = vr.DepartmentName,
                    UserName = vr.UserName,
                    WhatsappMobileNumber = vr.WhatsappMobileNumber,
                    VehicleFullNumber = vr.VehicleFullNumber,
                    VehicleType = vr.VehicleType,
                    TravellingFromTO = vr.TravellingFromTO,
                    DepartmentInChargePermission = vr.DepartmentInChargePermission,
                    RechargeAmount = vr.RechargeAmount,
                    DateOfRequest = vr.DateOfRequest,
                    ReferenceNo = vr.ReferenceNo,
                    Status = vr.Status,
                    Expiration = vr.Expiration,
                    FastagAccount = vr.FastagAccount,
                    Remarks = vr.Remarks
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