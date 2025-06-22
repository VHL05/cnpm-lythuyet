using QuanLyPhongKhamAnTam.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongKhamAnTam.Areas.Staff.ViewModels;

namespace QuanLyPhongKhamAnTam.Areas.Staff.Controllers
{
    //[Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        public QLPK_Nhom34_T5C2Entities db = new QLPK_Nhom34_T5C2Entities();
        // GET: Staff/Receptionist
        public ActionResult Index()
        {
            var appointments = db.Appointments
                               .Include(a => a.Patient)
                               .Include(a => a.Doctor)
                               .OrderByDescending(a => a.AppointmentDate)
                               .ToList();
            return View(appointments);
        }

        //Tao lich hen
        public ActionResult Create()
        {
            var model = new AppointmentViewModel
            {
                Doctors = new SelectList(db.Doctors, "DoctorID", "FullName"),
                Patients = new SelectList(db.Patients, "PatientID", "FullName"),
                AppointmentDate = DateTime.Now.AddHours(1)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    PatientID = model.PatientID,
                    DoctorID = model.DoctorID,
                    AppointmentDate = model.AppointmentDate,
                    Status = "Scheduled",
                    Notes = model.Notes
                };

                db.Appointments.Add(appointment);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã đặt lịch hẹn thành công!";
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, tải lại danh sách dropdown
            model.Doctors = new SelectList(db.Doctors, "DoctorID", "FullName", model.DoctorID);
            model.Patients = new SelectList(db.Patients, "PatientID", "FullName", model.PatientID);
            return View(model);
        }

        //Xem chi tiết lịch hẹn 
        public ActionResult Details(int id)
        {
            var appointment = db.Appointments
                              .Include(a => a.Patient)
                              .Include(a => a.Doctor)
                              .FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        //Cập nhật lịch hẹn 
        public ActionResult Edit(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            var model = new AppointmentViewModel
            {
                AppointmentID = appointment.AppointmentID,
                PatientID = appointment.PatientID,
                DoctorID = appointment.DoctorID,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes,
                Doctors = new SelectList(db.Doctors, "DoctorID", "FullName", appointment.DoctorID),
                Patients = new SelectList(db.Patients, "PatientID", "FullName", appointment.PatientID)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = db.Appointments.Find(model.AppointmentID);
                if (appointment == null)
                {
                    return HttpNotFound();
                }

                appointment.PatientID = model.PatientID;
                appointment.DoctorID = model.DoctorID;
                appointment.AppointmentDate = model.AppointmentDate;
                appointment.Status = model.Status;
                appointment.Notes = model.Notes;

                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã cập nhật lịch hẹn thành công!";
                return RedirectToAction("Index");
            }

            model.Doctors = new SelectList(db.Doctors, "DoctorID", "FullName", model.DoctorID);
            model.Patients = new SelectList(db.Patients, "PatientID", "FullName", model.PatientID);
            return View(model);
        }

        //Hủy lịch hẹn 
        public ActionResult Cancel(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            appointment.Status = "Cancelled";
            db.Entry(appointment).State = EntityState.Modified;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đã hủy lịch hẹn thành công!";
            return RedirectToAction("Index");
        }

        //Xem lịch hẹn theo ngày 
        public ActionResult ViewByDate(DateTime? date)
        {
            DateTime selectedDate = date ?? DateTime.Today;

            var appointments = db.Appointments
                              .Include(a => a.Patient)
                              .Include(a => a.Doctor)
                              .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == selectedDate.Date)
                              .OrderBy(a => a.AppointmentDate)
                              .ToList();

            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
            return View(appointments);
        }
    }
}