using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace eDraw.api.ServiceClient
{
    public class AwsMailClient : IAwsMailClient
    {
        public async Task<string> SendEmails()
        {
            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            const string from = "no-replay@edraw.com";
            const string fromname = "";

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            const string to = "matt@thriftmatch.com";

            // Replace smtp_username with your Amazon SES SMTP user name.
            const string smtpUsername = "api@edrawapi.thriftmatch.com";

            // Replace smtp_password with your Amazon SES SMTP user name.
            const string smtpPassword = "edraw@987";

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            const string configset = "ConfigSet";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate Region.
            const string host = "18.219.17.102";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            const int port = 587;

            // The subject line of the email
            const string subject =
                "Amazon SES test (SMTP interface accessed using C#)";

            // The body of the email
            const string body =
                "<h1>Amazon SES Test</h1>" +
                "<p>This email was sent through the " +
                "<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
                "using the .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromname)
            };
            message.To.Add(new MailAddress(to));
            message.To.Add(new MailAddress("danijel@thriftmatch.com"));
            message.Subject = subject;
            message.Body = body;
            // Comment or delete the next line if you are not using a configuration set
            message.Headers.Add("X-SES-CONFIGURATION-SET", configset);

            // Create and configure a new SmtpClient
            var client =
                new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = false
                };
            // Pass SMTP credentials
            // Enable SSL encryption

            // Send the email. 
            try
            {
                Console.WriteLine("Attempting to send email...");
                client.Send(message);
                Console.WriteLine("Email sent!");
                return await Task.FromResult("Email sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
                return await Task.FromResult(ex.Message);

            }
        }
    }
}
