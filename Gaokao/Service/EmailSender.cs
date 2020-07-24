using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
//using SendGrid;
//using SendGrid.Helpers.Mail;
using Gaokao.Service.SendCloud;
using Microsoft.Extensions.Configuration;
using Microsoft.CodeAnalysis.Options;

namespace Gaokao.Service
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            if(Options.SendCloudApiKey==null || Options.SendCloudApiUser==null
                || Options.SendCloudApiUser=="" || Options.SendCloudApiKey=="")
            {
                Options.SendCloudApiUser = Environment.GetEnvironmentVariable("SendCloudApiUser");
                Options.SendCloudApiKey = Environment.GetEnvironmentVariable("SendCloudApiKey");
            }
            Console.WriteLine(Options.SendCloudApiKey);
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute( Options.SendCloudApiUser, Options.SendCloudApiKey, subject, message, email);
            //return Execute(ApiKey, subject, message, email);
        }

        private Task Execute( string apiUser,string apiKey, string subject, string message, string email)
        {
            var client = new SendCloudClient( apiUser ,apiKey);
            var msg = new SendCloudMessage()
            {
                //From = new EmailAddress("vankyle@126.com", Options.SendGridUser),
                From = new EmailAddress("vankyle@126.com", "Vankyle"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));


            return client.SendEmailAsync(msg);
        }
    }
}
