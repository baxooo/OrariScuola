using System.Net.Mail;
using System.Net;

namespace OrariScuola
{
    public static class EmailSender 
    {
        public static async Task SendEmailAsync(string emailReceiver, 
                                                string emailSender, 
                                                string senderPassword, 
                                                string subject, 
                                                string messageBody,
                                                string attatchmentPath)
        {
            Attachment attachment = new(attatchmentPath);
            var message = new MailMessage();
            message.From = new MailAddress(emailSender, "me");
            message.To.Add(emailReceiver);
            message.Subject = subject;
            message.Body = messageBody;
            message.Attachments.Add(attachment);

            var client = new SmtpClient("smtp.gmail.com", 587);

            client.SendCompleted += (s, e) =>
            {
                client.Dispose();
                message.Dispose();
            };

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailSender, senderPassword);
            await client.SendMailAsync(message);
        }
    }
}
