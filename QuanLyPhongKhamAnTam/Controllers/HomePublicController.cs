using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongKhamAnTam.Controllers
{
    public class HomePublicController : Controller
    {
        // GET: HomePublic
        public ActionResult TrangchuPK()
        {
            var cookie = Request.Cookies["LoginCookie"];
            if (cookie != null && cookie["Username"] != null)
            {
                ViewBag.Username = cookie["Username"];
            }
            return View();
        }

        public ActionResult DichvuPK()
        {
            return View();
        }
        public ActionResult GioithieuPK()
        {
            return View();
        }
        public ActionResult KhamBHYT()
        {
            return View();
        }
        public ActionResult KhamSK()
        {
            return View();
        }
        public ActionResult Noisoi()
        {
            return View();
        }
        public ActionResult KhamSKDHDL()
        {
            return View();
        }
        public ActionResult TiemChung()
        {
            return View();
        }
        public ActionResult Noitongquat()
        {
            return View();
        }
        public ActionResult Ngoaitongquat()
        {
            return View();
        }
        public ActionResult Taimuihong()
        {
            return View();
        }
        public ActionResult Ranghammat()
        {
            return View();
        }
        public ActionResult Nhikhoa()
        {
            return View();
        }
        public ActionResult Sanphukhoa()
        {
            return View();
        }
        public ActionResult YHocCoTruyen()
        {
            return View();
        }
    }
}