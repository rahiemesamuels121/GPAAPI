using GPACARICOMAPI.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace GPACARICOMAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration) {
            _configuration = configuration;
        }


        public async Task sendEmail(String reciever, string subject, string body) {

            var email = _configuration.GetValue<String>("EmailConfiguration:Email");
            var password = _configuration.GetValue<String>("EmailConfiguration:Password");
            var host = _configuration.GetValue<String>("EmailConfiguration:Host");
            var port = _configuration.GetValue<int>("EmailConfiguration:Port");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential(email, password);
            using var message = new MailMessage(email!, reciever, subject, body)
            {
                IsBodyHtml = true
            };
            await smtpClient.SendMailAsync(message);


        }
    }
}
