using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace UniqueStudio.Common.Mailing
{
    /// <summary>
    /// 邮件发送类
    /// </summary>
    /// <remarks>该类尚未实现功能</remarks>
    public class SMTP
    {
        public SMTP()
        {
        }

        public void Send(string host,int port,bool enableSsl,string userName,string password,string to,string subject,string body)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = enableSsl;
            smtp.Credentials = new NetworkCredential(userName, password);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("imacen@qq.com");
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;
            smtp.Send(mail);
        }
    }
}
