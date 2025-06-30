using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services 
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Smtp:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["Smtp:Host"],
                int.Parse(_config["Smtp:Port"]),
                SecureSocketOptions.StartTls // ✅ <-- Use this for port 587!
            );

            await smtp.AuthenticateAsync(
                _config["Smtp:Username"],
                _config["Smtp:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
