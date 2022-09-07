using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CandidatesBrowserEngine.Utilities
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
     

        public MailJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            MailjetClient client = new MailjetClient(_configuration.GetSection("MailJetApiKey").Value, _configuration.GetSection("MailJetSecretKey").Value);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
           .Property(Send.FromEmail, _configuration.GetSection("MailJetEmail").Value)
           .Property(Send.FromName, _configuration.GetSection("MailJetSender").Value)
           .Property(Send.Subject, subject)
           .Property(Send.HtmlPart, htmlMessage)
           .Property(Send.Recipients, 
                new JArray {
                    new JObject {{"Email", email}}
                            });
              var result=await client.PostAsync(request);
                
        }
    }
}
