using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongKhamAnTam.Areas.Staff.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        // Danh sách bác sĩ và bệnh nhân cho dropdown
        public SelectList Doctors { get; set; }
        public SelectList Patients { get; set; }
    }
}