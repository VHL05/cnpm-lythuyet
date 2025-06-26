using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyPhongKhamAnTam.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Mã xác minh")]
        public string VerificationCode { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }

        // Role không hiển thị trong View, nhưng sẽ được gán mặc định trong Controller
        public string Role { get; set; } = "Patient";

        public int? LinkedDoctorID { get; set; } // Nếu không dùng, có thể bỏ
    }
}
