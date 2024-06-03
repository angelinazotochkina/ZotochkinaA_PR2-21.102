using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZotochkinaA_PR2_21._102
{
    internal class MailRuMailSender
    {

        public static string SendMailRu(string userEmail)
        {
            string mailRuAppPassword = "dJxhnqMiPf9cThuH2Ck8";
            MailMessage mail = new MailMessage(); // Создаем объект сообщения
            mail.From = new MailAddress("lina.zotochkina@mail.ru", "Магазин одежды \"ООО Синар\"");
            mail.To.Add("lina.zotochkina@mail.ru"); // Добавляем получателя письма
            mail.Subject = "Код подтверждения"; // тема письма
            string confirmationCode = GenerateFourDigitCode();
            mail.Body = $"Ваш код подтверждения: {confirmationCode}";
            SmtpClient smtpClient = new SmtpClient("smtp.mail.ru"); // Создаем клиента SMTP для отправки письма
            smtpClient.Port = 587; // устанавливаем порт 
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("lina.zotochkina@mail.ru", mailRuAppPassword);
            try
            {
                smtpClient.Send(mail);
                MessageBox.Show("Письмо успешно отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return confirmationCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при отправке письма: " + ex.Message);
                return null;
            }
        }
        private static string GenerateFourDigitCode()
        {
            Random random = new Random();
            int code = random.Next(1000, 10000);
            return code.ToString("D4");
        }
    }
}

