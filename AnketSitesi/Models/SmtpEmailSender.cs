using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;

namespace AnketSitesi.Models
{
    public class SmtpEmailSender : IEmailSender
    {

        private string? _host;

        private int _port;

        private bool _enableSSL;

        private string? _password;

        private string? _username;
        public SmtpEmailSender(string? host, int port, bool enableSSL, string? password, string? username)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _password = password;

            _username = username;

        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSL
            };

           return client.SendMailAsync(new MailMessage(_username ?? "", email, subject, message) { IsBodyHtml=true });
        }
    }
}
