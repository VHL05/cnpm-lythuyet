using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongKhamAnTam.Areas.Staff.Controllers
{
    public class StaffHomeController : Controller
    {
        // GET: Staff/StaffHome
        public ActionResult HomeStaff()
        {
            return View();
        }
    }
}