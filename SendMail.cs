using System;
using System.Net;
using System.Net.Mail;

namespace CADShark.OpenCAD.Addin
{
    internal class SendMail
    {
        internal static void Main()
        {
            var fromAddress = "your_email@gmail.com";
            var toAddress = "recipient_email@example.com";

            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;
            
            var username = "your_email@gmail.com";
            var password = new NetworkCredential("", "your_password").SecurePassword;
            
            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = "Тестовое письмо";
            message.Body = "Привет, это тестовое письмо!";

            var smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(username, password);

            try
            {
                smtpClient.Send(message);
                Console.WriteLine("Письмо успешно отправлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки письма: {ex.Message}");
            }
        }
    }
}
