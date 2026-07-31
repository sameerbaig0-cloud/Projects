using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ServicePlatform.Interface
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string ccEmail, string subject, string body);
    }


    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;

        public SmtpEmailService(IConfiguration configuration)
        {
            IConfigurationSection smtpSettings = configuration.GetSection("SmtpSettings");
            _senderEmail = smtpSettings["SenderEmail"];

            _smtpClient = new SmtpClient
            {
                Host = smtpSettings["Host"],
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            };
        }

        public async Task SendEmailAsync(string toEmail, string ccEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            mailMessage.CC.Add(ccEmail);

            try
            {
               await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                throw new ApplicationException($"Email could not be sent. Error: {ex.Message}");
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
    }
}
