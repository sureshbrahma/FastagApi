using FasTagApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FasTagApi.Controllers
{
    public class HomeController : Controller
    {
        private FasTagEntities1 db = new FasTagEntities1();

        // GET: VehicleRequests
        public Task<ActionResult> IndexImage()
        {
            return Task.FromResult<ActionResult>(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexImage(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Here you can implement your login logic
                if (model.Username == "bkaccfastag" && model.Password == "bkaccanmol")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (model.Username == "IOSFAS" && model.Password == "IOSFAS")
                {
                    return RedirectToAction("Index", "IOS");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> IndexMobile()
        {
            return View(await db.MobileChanges.ToListAsync());
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.VehicleRequests.ToListAsync());
        }

        public async Task<ActionResult> EditM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileChange mobileRequest = await db.MobileChanges.FindAsync(id);
            if (mobileRequest == null)
            {
                return HttpNotFound();
            }
            return View(mobileRequest);
        }

        // POST: VehicleRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditM([Bind(Include = "S_NO,VehicleNumber,OldMobileNumber,NewMobileNumber,Status,Remarks")] MobileChange vehicleRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Fastag Details Edited Successfully";
                return RedirectToAction("IndexMobile");
            }
            return View(vehicleRequest);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRequest vehicleRequest = await db.VehicleRequests.FindAsync(id);
            if (vehicleRequest == null)
            {
                return HttpNotFound();
            }
            return View(vehicleRequest);
        }

        // POST: VehicleRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Sno,InstitutionName,DepartmentName,UserName,WhatsappMobileNumber,VehicleFullNumber,VehicleType,TravellingFromTO,DepartmentInChargePermission,RechargeAmount,DateOfRequest,ReferenceNo,Status,Expiration,FastagAccount,Remarks")] VehicleRequest vehicleRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Fastag Details Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(vehicleRequest);
        }

        public async Task<ActionResult> DeleteM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileChange vehicleRequest = await db.MobileChanges.FindAsync(id);
            if (vehicleRequest == null)
            {
                return HttpNotFound();
            }
            return View(vehicleRequest);
        }

        [HttpPost, ActionName("DeleteM")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedM(int id)
        {
            MobileChange vehicleRequest = await db.MobileChanges.FindAsync(id);
            db.MobileChanges.Remove(vehicleRequest);
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Fastag Details Deleted Successfully";
            return RedirectToAction("IndexMobile");
        }

        // GET: VehicleRequests/Delete/5

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRequest vehicleRequest = await db.VehicleRequests.FindAsync(id);
            if (vehicleRequest == null)
            {
                return HttpNotFound();
            }
            return View(vehicleRequest);
        }

        // POST: VehicleRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VehicleRequest vehicleRequest = await db.VehicleRequests.FindAsync(id);
            db.VehicleRequests.Remove(vehicleRequest);
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Fastag Details Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}