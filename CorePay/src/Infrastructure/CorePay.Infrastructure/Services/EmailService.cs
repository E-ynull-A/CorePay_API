using CorePay.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Infrastructure.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail
                                   ,string subject
                                   ,string body)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["SMTP:Host"],
                                                   int.Parse(_configuration["SMTP:Port"]));

            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_configuration["SMTP:LoginEmail"],
                                                           _configuration["SMTP:Password"]); 

            MailAddress from = new MailAddress(_configuration["SMTP:LoginEmail"],"CorePayBank");
            MailAddress to = new MailAddress(toEmail); 


            MailMessage message = new MailMessage(from,to);

            message.Subject = subject;
            message.Body = body;
                    
            await smtpClient.SendMailAsync(message);
        }
    }
}
