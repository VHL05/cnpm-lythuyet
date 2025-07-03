using QuanLyPhongKhamAnTam.Models;
using QuanLyPhongKhamAnTam.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
namespace QuanLyPhongKhamAnTam.Controllers
{
    public class AccountController : Controller
    {
        private readonly QLPK_Nhom34_T5C2Entities db = new QLPK_Nhom34_T5C2Entities();

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string sentCode = Session["VerificationCode"] as string;
            string verifiedEmail = Session["VerifiedEmail"] as string;

            if (model.Email != verifiedEmail || model.VerificationCode != sentCode)
            {
                ViewBag.Error = "Bạn cần xác minh email trước khi đăng ký.";
                ViewBag.ShowVerification = true;
                return View(model);
            }

            // ✅ Gán mặc định nếu Role null hoặc rỗng
            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = "Patient";
            }

            var account = new Account
            {
                Username = model.Username,
                PasswordHash = model.Password,
                Email = model.Email,
                Role = model.Role,
                LinkedDoctorID = model.LinkedDoctorID
            };

            try
            {
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("✖ Lỗi tại thuộc tính: " + ve.PropertyName + " — " + ve.ErrorMessage);
                    }
                }

                ViewBag.Error = "Lỗi khi lưu tài khoản. Vui lòng kiểm tra lại thông tin.";
                return View(model);
            }

            Session.Remove("VerificationCode");
            Session.Remove("VerifiedEmail");

            TempData["Message"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }



        [HttpPost]
        public async Task<JsonResult> SendVerificationCode(string email)
        {
            try
            {
                string code = new Random().Next(100000, 999999).ToString();

                var emailService = new EmailService();
                await emailService.SendVerificationCodeAsync(email, code);

                Session["VerificationCode"] = code;
                Session["VerifiedEmail"] = email;

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult VerifyCode(RegisterViewModel model)
        {
            string correctCode = TempData["VerificationCode"] as string;
            if (model.VerificationCode != correctCode)
            {
                ModelState.AddModelError("VerificationCode", "Mã xác minh không đúng.");
                return View(model);
            }

            var account = new Account
            {
                Username = model.Username,
                PasswordHash = model.Password,
                Role = model.Role,
                LinkedDoctorID = model.LinkedDoctorID
            };

            db.Accounts.Add(account);
            db.SaveChanges();

            return RedirectToAction("Login");
        }


        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Accounts.FirstOrDefault(a =>
                a.Username.Trim() == model.Username.Trim() &&
                a.PasswordHash.Trim() == model.Password.Trim());

            if (user != null)
            {
                // Tạo Cookie lưu Username (KHÔNG lưu mật khẩu!)
                HttpCookie cookie = new HttpCookie("LoginCookie");
                cookie["Username"] = user.Username;
                cookie.Expires = DateTime.Now.AddDays(7); // Cookie tồn tại 7 ngày
                Response.Cookies.Add(cookie);

                return RedirectToAction("TrangChuPK", "HomePublic");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(model);
        }
        public ActionResult Logout()
        {
            if (Request.Cookies["LoginCookie"] != null)
            {
                var cookie = new HttpCookie("LoginCookie");
                cookie.Expires = DateTime.Now.AddDays(-1); // Xóa cookie
                Response.Cookies.Add(cookie);
            }

            Session.Clear(); // Xóa session (nếu dùng)

            return RedirectToAction("Login", "Account");
        }

    }
}
