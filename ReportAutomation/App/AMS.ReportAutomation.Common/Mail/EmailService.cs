using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AMS.ReportAutomation.Common.Mail
{
    public class EmailService
    {
        public void SendNotification(string content)
        {
            using (var client = GetClient())
            using (var message = CreateNotificationMail(content))
            {
                client.Send(message);
            }
        }

        public void Send(string subject, string content, List<string> receiverEmails)
        {
            using (var client = GetClient())
            using (var message = CreateMail(subject, content, receiverEmails))
            {
                client.Send(message);
            }
        }

        private SmtpClient GetClient()
        {
            //Create an instance of the SMTP transport mechanism
            return new SmtpClient
            {
                Host = MailSettings.Host,
                Port = MailSettings.Port,
                UseDefaultCredentials = MailSettings.UseDefaultCredentials,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(MailSettings.Username, MailSettings.Password),
                EnableSsl = true
            };
        }

        private MailMessage CreateNotificationMail(string content)
        {
            var mailTos = MailSettings.To.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return CreateMail(MailSettings.MailSubject, content, mailTos);
        }

        private MailMessage CreateMail(string subject, string content, IEnumerable<string> receiverEmails)
        {
            //Create a new message object
            var message = new MailMessage();
            message.From = new MailAddress(MailSettings.From);
            foreach (string recipient in receiverEmails)
            {
                message.To.Add(recipient);
            }
            message.Subject = subject;
            message.Body = content;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            return message;
        }
    }
}