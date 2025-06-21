using System.Web.Mvc;
namespace QuanLyPhongKhamAnTam.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            // Logic to authenticate user goes here
            if (username == "admin" && password == "password") // Example condition
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }
    }
}