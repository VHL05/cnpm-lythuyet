using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    public async Task SendVerificationCodeAsync(string toEmail, string code)
    {
        var fromEmail = ConfigurationManager.AppSettings["EmailAddress"];
        var password = ConfigurationManager.AppSettings["EmailPassword"];

        var message = new MailMessage();
        message.From = new MailAddress(fromEmail, "Phòng khám An Tâm");
        message.To.Add(toEmail);
        message.Subject = "Mã xác minh đăng ký tài khoản";
        message.Body = $"Mã xác minh của bạn là: {code}";
        message.IsBodyHtml = false;

        using (var smtp = new SmtpClient("smtp.gmail.com", 587))
        {
            smtp.Credentials = new NetworkCredential(fromEmail, password);
            smtp.EnableSsl = true;

            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi SMTP: " + ex.Message);
            }
        }
    }

}
