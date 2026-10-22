using CorePay.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CorePayBank", _configuration["SMTP:LoginEmail"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();

            // Port 587 üçün StartTls işlədilir
            await client.ConnectAsync(
                _configuration["SMTP:Host"],
                int.Parse(_configuration["SMTP:Port"]),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _configuration["SMTP:LoginEmail"],
                _configuration["SMTP:Password"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
