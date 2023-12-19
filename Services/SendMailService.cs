using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace CuaHangDoAn.Services
{
    public class SendMailService : IEmailSender
    {
        private IConfiguration _configuration;
        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress("Gamehay", "qhao74155@gmail.com");
            message.From.Add(new MailboxAddress("Gamehay2", "qhao74155@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = html };

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetValue<string>("MailSettings:Mail"), _configuration.GetValue<string>("MailSettings:Password"));
            smtp.Send(message);
            smtp.Disconnect(true);
        }
    }
}
