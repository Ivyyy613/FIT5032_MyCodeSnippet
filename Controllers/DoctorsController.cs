using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_EasyX.Models;
using FIT5032_EasyX.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FIT5032_EasyX.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private Entities db = new Entities();
        private ApplicationUserManager _userManager;

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

        // GET: Doctors
        public ActionResult Index()
        {
            var doctorRatings = db.BookingsSet
                .Where(b => b.Rating > 0)
                .GroupBy(b => b.DoctorId)
                .Select(g => new
                {
                    DoctorId = g.Key,
                    AverageRating = g.Average(b => b.Rating)
                })
                .ToDictionary(d => d.DoctorId, d => d.AverageRating);

            ViewBag.DoctorRatings = doctorRatings;
            return View(db.DoctorSet.ToList());
        }

        // GET: Doctors/Details/5
        [Authorize(Roles ="admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.DoctorSet.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }
        [Authorize(Roles = "admin,doctor")]
        public ActionResult MyDetails()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.DoctorSet.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //// GET: Doctors/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Doctors/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,First_Name,Last_Name,Major,Address,Is_Ready_For_Visitor")] Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AspNetUsers.Add(doctor);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(doctor);
        //}

        // GET: Doctors/Create/5
        [Authorize]

        public ActionResult Create(string id)
        {
            if (id == null || id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = new Doctor()
            {
                Id = id
            };
            return View(doctor);
        }

        // POST: Doctors/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,First_Name,Last_Name,Major,Address,Is_Ready_For_Visitor")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                AspNetUsers aspNetUsers = db.AspNetUsers.Find(doctor.Id);
                db.AspNetUsers.Remove(aspNetUsers);

                Doctor doctorObj = new Doctor(aspNetUsers, doctor);
                db.DoctorSet.Add(doctorObj);
                db.SaveChanges();
                // Assign doctor role to this new doctor
                UserManager.AddToRole(doctorObj.Id, StaticRole.doctor);
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "admin,doctor")]

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.DoctorSet.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,doctor")]

        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,First_Name,Last_Name,Major,Address,Is_Ready_For_Visitor")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                if (User.IsInRole("doctor"))                
                    return RedirectToAction("MyDetails");
                if (User.IsInRole("admin"))
                    return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "admin,doctor")]

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.DoctorSet.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,doctor")]

        public ActionResult DeleteConfirmed(string id)
        {
            Doctor doctor = db.DoctorSet.Find(id);
            db.AspNetUsers.Remove(doctor);
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
