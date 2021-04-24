using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Results;

namespace Core.Helper
{
    public class SendMail
    {
        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public static IServiceResult SendMailProcess(string subject, string body, string email)
        {
            var smtpAddress = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpUser");
            var smtpPassword = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpPassword");
            var smtpSendPort = Convert.ToInt32(Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpSendPort"));
            var smtpHost = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpServer");


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpAddress);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpSendPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpAddress, smtpPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return new ServiceResult(true,"ok");
        }
    }
}
