using QuanLyPhongKhamAnTam.Models;
using QuanLyPhongKhamAnTam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace QuanLyPhongKhamAnTam.Controllers
{
    public class AppointmentController : Controller
    {
        public QLPK_Nhom34_T5C2Entities db = new QLPK_Nhom34_T5C2Entities();

        // GET: Đặt lịch hẹn công khai
        public ActionResult Create()
        {
            var model = new AppointmentViewModel
            {
                Doctors = new SelectList(db.Doctors, "DoctorID", "FullName"),
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
                // Tìm hoặc tạo bệnh nhân
                var patient = db.Patients.FirstOrDefault(p => p.Phone == model.PatientPhone);

                if (patient == null)
                {
                    patient = new Patient
                    {
                        FullName = model.PatientName,
                        Phone = model.PatientPhone,
                        Email = model.PatientEmail,
                        Gender = "Other",
                        DateOfBirth = DateTime.Now.AddYears(-20), // Mặc định
                        Address = "Tự đặt lịch online"
                    };
                    db.Patients.Add(patient);
                    db.SaveChanges();
                }

                // Tạo lịch hẹn
                var appointment = new Appointment
                {
                    PatientID = patient.PatientID,
                    DoctorID = model.DoctorID,
                    AppointmentDate = model.AppointmentDate,
                    Status = "Scheduled",
                    Notes = $"Tự đặt online. Triệu chứng: {model.Symptoms}"
                };

                db.Appointments.Add(appointment);
                db.SaveChanges();

                return RedirectToAction("Success", new { id = appointment.AppointmentID });
            }

            // Nếu có lỗi, load lại danh sách bác sĩ
            model.Doctors = new SelectList(db.Doctors, "DoctorID", "FullName", model.DoctorID);
            return View(model);
        }

        public ActionResult Success(int id)
        {
            var appointment = db.Appointments
                              .Include(a => a.Doctor)
                              .Include(a => a.Patient)
                              .FirstOrDefault(a => a.AppointmentID == id);

            // Gửi SMS (giả lập)
            SendSMS(appointment.Patient.Phone,
                   $"An Tâm Clinic: Bạn đã đặt lịch thành công #{id} vào {appointment.AppointmentDate:dd/MM HH:mm}");

            return View(appointment);
        }

        private void SendSMS(string phone, string message)
        {
            // Triển khai gửi SMS thực tế ở đây
            Debug.WriteLine($"Đã gửi SMS tới {phone}: {message}");
        }
        // GET: Tra cứu lịch hẹn
        public ActionResult CheckAppointment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckAppointment(string phone)
        {
            var appointments = db.Appointments
                               .Include(a => a.Doctor)
                               .Where(a => a.Patient.Phone == phone)
                               .OrderByDescending(a => a.AppointmentDate)
                               .ToList();

            if (!appointments.Any())
            {
                ViewBag.Message = "Không tìm thấy lịch hẹn nào cho số điện thoại này";
                return View();
            }

            return View("AppointmentList", appointments);
        }

        public ActionResult AppointmentList(List<Appointment> appointments)
        {
            return View(appointments);
        }
    }
}