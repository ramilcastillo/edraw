using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using eDraw.api.Core.Models;
using eDraw.api.Core.Models.AppSettings;
using Microsoft.Extensions.Options;

namespace eDraw.api.ServiceClient
{
    public class EmailServiceClient : IEmailServiceClient
    {
        private readonly EmailSettings _emailSettings;

        public EmailServiceClient(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(EmailProperties emailProperties)
        {
            //const string configset = "ConfigSet";
            var status = false;
            const string awsAccessKey = "AKIAJ4IK5ITYGVQ6NICA";
            const string awsAccessSecretKey = "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";

            using (var awsClient = new AmazonSimpleEmailServiceClient(awsAccessKey,awsAccessSecretKey,RegionEndpoint.USEast1))
            {
                var request = new SendEmailRequest
                {
                    Source = _emailSettings.SmtpEmail,
                    Destination = new Destination()
                    {
                       ToAddresses = emailProperties.ReceipentsEmail
                    },
                    Message = new Message()
                    {
                        Subject = new Content(emailProperties.Subject),
                        Body = new Body()
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = "This message body contains HTML formatting. It can, for example, contain links like this one: <a href ='http://docs.aws.amazon.com/ses/latest/DeveloperGuide\' target = '\'_blank\"> Amazon SES Developer Guide </a>."
                            },
                            Text = new Content()
                            {
                                Charset = "UTF-8",
                                Data = emailProperties.Body
                            }
                        }
                    }
                };

                var templatedEmailRequest = new SendTemplatedEmailRequest
                {
                    Source = _emailSettings.SmtpEmail,
                    Destination = new Destination()
                    {
                        ToAddresses = emailProperties.ReceipentsEmail
                    },
                    Template = "withButtonTemplate",
                    TemplateData = "{\"subject\":\""+emailProperties.Subject+"\"}"
                };

                var response = await awsClient.SendEmailAsync(request);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    status = true;
                }
            }


            return status;
        }

        public bool IsValidEmail(string email)
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
