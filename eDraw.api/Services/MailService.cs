using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace eDraw.api.Services
{
    public class MailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool SendMail(string subject, IEnumerable<string> receipents, string template)
        {
            try
            {
                var credential = new NetworkCredential(_configuration["SmtpEmail"], _configuration["SmtpPassword"]);
                var client = new SmtpClient
                {
                    Host = _configuration["Host"],
                    Port = int.Parse(_configuration["Port"]),
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = credential
                };
                var msg = new MailMessage
                {
                    From = new MailAddress(_configuration["MailAddress"], _configuration["DisplayName"]),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = template
                };
                foreach (var email in receipents)
                {
                    if (IsValidEmail(email)) msg.To.Add(new MailAddress(email));
                }
                if (msg.To.Count > 0)
                {
                    client.Send(msg);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
