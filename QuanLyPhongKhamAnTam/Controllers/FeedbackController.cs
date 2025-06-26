using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongKhamAnTam.Models;

namespace QuanLyPhongKhamAnTam.Controllers
{
    public class FeedbackController : Controller
    {
        private QLPK_Nhom34_T5C2Entities1 db = new QLPK_Nhom34_T5C2Entities1();

        // GET: Feedback/Index
        // Hiển thị danh sách feedback
        public ActionResult IndexFeedBack()
        {
            var feedbacks = db.Feedbacks
                .Include(f => f.Specialty)
                .OrderByDescending(f => f.DateSubmitted)
                .ToList();
            return View(feedbacks);
        }

        // GET: Feedback/Create
        // Hiển thị form tạo feedback
        public ActionResult Create()
        {
            ViewBag.SpecialtyID = new SelectList(db.Specialties, "SpecialtyID", "SpecialtyName");
            return View();
        }

        // POST: Feedback/Create
        // Xử lý tạo feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SpecialtyID,Content,Name")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.DateSubmitted = DateTime.Today;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("IndexFeedBack");
            }

            ViewBag.SpecialtyID = new SelectList(db.Specialties, "SpecialtyID", "SpecialtyName", feedback.SpecialtyID);
            return View(feedback);
        }

        // GET: Feedback/Edit/5
        // Hiển thị form sửa feedback
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.SpecialtyID = new SelectList(db.Specialties, "SpecialtyID", "SpecialtyName", feedback.SpecialtyID);
            return View(feedback);
        }

        // POST: Feedback/Edit/5
        // Xử lý sửa feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,SpecialtyID,Content,Name,DateSubmitted")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpecialtyID = new SelectList(db.Specialties, "SpecialtyID", "SpecialtyName", feedback.SpecialtyID);
            return View(feedback);
        }

        // GET: Feedback/Delete/5
        // Hiển thị form xóa feedback
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedback/Delete/5
        // Xử lý xóa feedback
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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