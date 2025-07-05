using System.Web;
using System.Web.Mvc;

namespace QuanLyPhongKhamAnTam.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Nếu Session chưa có thì lấy từ cookie
            if (Session["Username"] == null && Request.Cookies["LoginCookie"]?["Username"] != null)
            {
                Session["Username"] = Request.Cookies["LoginCookie"]["Username"];
            }

            // Luôn gán ViewBag.Username cho Layout
            if (Session["Username"] != null)
            {
                ViewBag.Username = Session["Username"].ToString();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
