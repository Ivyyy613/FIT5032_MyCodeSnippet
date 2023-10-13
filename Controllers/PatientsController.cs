using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_EasyX.Models;
using FIT5032_EasyX.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FIT5032_EasyX.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private Entities db = new Entities();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Patients
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.PatientSet.ToList());
        }
        [Authorize(Roles = "admin")]
        // GET: Patients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.PatientSet.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }
        [Authorize(Roles = "admin,patient")]
        public ActionResult MyDetails()
        {
            String id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.PatientSet.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }
        [Authorize]
        // GET: Patients/Create
        public ActionResult Create(string id)
        {
            if (id == null || id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = new Patient()
            {
                Id = id
            };
            return View(patient);
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Title,First_Name,Last_Name,Date_Of_Birth,Address")] Patient patient)

        //public async Task<ActionResult> Create([Bind(Include = "Id,Title,First_Name,Last_Name,Date_Of_Birth,Address")] Patient patient)
        {

            if (ModelState.IsValid)
            {
                //var externalLogin = UserManager.GetLogins(aspNetUsers.Id);
                var temp = db.AspNetUserLogins.Where(login => login.UserId == patient.Id).FirstOrDefault();

                AspNetUsers aspNetUsers = db.AspNetUsers.Find(patient.Id);
                db.AspNetUsers.Remove(aspNetUsers);
                Patient patientObj = new Patient(aspNetUsers, patient);

                db.PatientSet.Add(patientObj);
                //db.AspNetUserLogins.Add(temp);
                db.SaveChanges();
                if (temp != null)
                    db.AspNetUserLogins.Add(temp);
                db.SaveChanges();
                UserManager.AddToRole(patientObj.Id, StaticRole.patient);
                //await SignInManager.SignInAsync(aspNetUsers, isPersistent: false, rememberBrowser: false);
                //return RedirectToAction("Index");
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }

            return View(patient);
        }
        [Authorize(Roles = "admin,patient")]
        // GET: Patients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.PatientSet.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,patient")]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,First_Name,Last_Name,Date_Of_Birth,Address")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                if (User.IsInRole("patient"))
                    return RedirectToAction("MyDetails");
                if (User.IsInRole("admin"))
                    return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.PatientSet.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            Patient patient = db.PatientSet.Find(id);
            db.PatientSet.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
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
