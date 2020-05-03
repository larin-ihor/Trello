using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Trello
{
    class MailSender
    {
        public bool Send(string email, string subject, string body)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("larin.csharptest@gmail.com", "C-Sharp test");
                // кому отправляем
                MailAddress to = new MailAddress(email);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = subject;
                // текст письма
                m.Body = body;
                // письмо представляет код html
                m.IsBodyHtml = false;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("larin.csharptest@gmail.com", "65011212");
                smtp.EnableSsl = true;
                smtp.Send(m);

                return true;
            }
            catch (System.Net.Mail.SmtpException)
            {
                return false;
            }   
        }
    }
}
