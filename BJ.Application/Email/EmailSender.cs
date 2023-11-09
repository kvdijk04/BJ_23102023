using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BJ.Application.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message, string from, string nameFrom);
    }
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IConfiguration _configuration;
        public EmailSender(EmailConfiguration emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig;
            _configuration = configuration;
        }

        public void SendEmail(Message message, string from, string nameFrom)
        {
            var emailMessage = CreateEmailMessage(message, from, nameFrom);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message, string from, string nameFrom)
        {
            var emailMessage = new MimeMessage();
            _emailConfig.From = from;
            emailMessage.From.Add(new MailboxAddress(nameFrom, from));
            emailMessage.To.Add(MailboxAddress.Parse(_configuration.GetValue<string>("EmailConfiguration:To")));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

