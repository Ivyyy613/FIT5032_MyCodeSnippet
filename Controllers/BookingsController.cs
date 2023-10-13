using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_EasyX.Models;
using FIT5032_EasyX.Util;
using Microsoft.AspNet.Identity;

namespace FIT5032_EasyX.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private Entities db = new Entities();

        // GET: Bookings
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            if (User.IsInRole(StaticRole.admin))
            {
                var bookingsSetAdmin = db.BookingsSet.Include(b => b.Doctor).Include(b => b.Patient);
                return View(bookingsSetAdmin.ToList());
            }
            if (User.IsInRole(StaticRole.doctor))
            {
                var bookingsSetAdmin = db.BookingsSet.Include(b => b.Doctor).Include(b => b.Patient).Where(x => x.DoctorId == id);
                return View(bookingsSetAdmin.ToList());
            }
            if (User.IsInRole(StaticRole.patient))
            {
                var bookingsSetAdmin = db.BookingsSet.Include(b => b.Doctor).Include(b => b.Patient).Where(x => x.PatientId == id);
                return View(bookingsSetAdmin.ToList());
            }

            var bookingsSet = db.BookingsSet.Include(b => b.Doctor).Include(b => b.Patient);
            return View(bookingsSet.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.BookingsSet.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(StaticRole.doctor) && bookings.DoctorId == currentUserId)
            {
                return View(bookings);
            }
            if (User.IsInRole(StaticRole.patient) && bookings.PatientId == currentUserId)
            {
                return View(bookings);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "patient")]
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.DoctorSet, "Id", "Email");
            //ViewBag.PatientId = new SelectList(db.PatientSet, "Id", "Email");
            var bookings = new Bookings()
            {
                PatientId = User.Identity.GetUserId(),
                Booking_Date = DateTime.Now,
                Booking_IsConfirm = false,
                Rating = 0
            };
            return View(bookings);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient")]
        public ActionResult Create([Bind(Include = "Booking_Id,Booking_Date,Booking_Content,Booking_IsConfirm,DoctorId,PatientId,Rating")] Bookings bookings)
        {
            //bookings.PatientId = User.Identity.GetUserId();
            //bookings.Booking_IsConfirm = false;
            //bookings.Rating = 0;
            //TryValidateModel(bookings);
            if (ModelState.IsValid)
            {
                db.BookingsSet.Add(bookings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            //ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            return View(bookings);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Edit(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.BookingsSet.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            if (User.IsInRole(StaticRole.doctor) && bookings.DoctorId == currentUserId)
            {
                return View(bookings);
            }
            if (User.IsInRole(StaticRole.patient) && bookings.PatientId == currentUserId)
            {
                return View(bookings);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Edit([Bind(Include = "Booking_Id,Booking_Date,Booking_Content,Booking_IsConfirm,DoctorId,PatientId,Rating")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            return View(bookings);
        }

        // GET: Bookings/Review/5
        [Authorize(Roles = "patient")]
        public ActionResult Review(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.BookingsSet.Find(id);
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(StaticRole.patient) && bookings.PatientId == currentUserId)
            {
                return View(bookings);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Bookings/Review/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient")]
        public ActionResult Review([Bind(Include = "Booking_Id,Booking_Date,Booking_Content,Booking_IsConfirm,DoctorId,PatientId,Rating")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            return View(bookings);
        }

        // GET: Bookings/Confirm/5
        [Authorize(Roles = "doctor")]
        public ActionResult Confirm(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.BookingsSet.Find(id);
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(StaticRole.doctor) && bookings.DoctorId == currentUserId)
            {
                return View(bookings);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Bookings/Confirm/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "doctor")]
        public ActionResult Confirm([Bind(Include = "Booking_Id,Booking_Date,Booking_Content,Booking_IsConfirm,DoctorId,PatientId,Rating")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.DoctorId);
            ViewBag.PatientId = new SelectList(db.AspNetUsers, "Id", "Email", bookings.PatientId);
            return View(bookings);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "admin,patient")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.BookingsSet.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,patient")]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookings bookings = db.BookingsSet.Find(id);
            db.BookingsSet.Remove(bookings);
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

        [Authorize(Roles = "patient")]
        public ActionResult Send_Email(string email)
        {
            SendEmailViewModel sendEmailViewModel = new SendEmailViewModel();
            sendEmailViewModel.ToEmail = email;
            sendEmailViewModel.Contents = "This is content";
            return View(sendEmailViewModel);
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //String toEmail = model.ToEmail;
                    //String subject = model.Subject;
                    //String contents = model.Contents;
                    //HttpPostedFileBase attachment = model.Attachment;

                    EmailSender es = new EmailSender();
                    //es.Send(toEmail, subject, contents, attachment);
                    es.Send("youivy2@gmail.com", model.Subject, model.Contents, model.Attachment);
                    es.Send(model.ToEmail, model.Subject, model.Contents, model.Attachment);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }
    }
}
