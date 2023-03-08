using Ferma.Data.Datacontext;
using Ferma.Service.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Security;
using MimeKit.Text;

namespace Ferma.Service.Services.Implementations
{
    public class EmailServices : IEmailServices
    {
        private readonly DataContext _context;

        public EmailServices(DataContext context)
        {
            _context = context;
        }
        public void Send(string to, string subject, string html)
        {
            //var context = _context..FirstOrDefault(x => x.Id == 1);

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("idagrouptester@yandex.ru"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("idagrouptester@yandex.ru", "idagroup123");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
