using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongKhamAnTam.ViewModels
{
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PatientPhone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string PatientEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bác sĩ")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày giờ")]
        public DateTime AppointmentDate { get; set; }

        public string Symptoms { get; set; }

        public SelectList Doctors { get; set; }
    }
}