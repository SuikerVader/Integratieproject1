using System;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Services
{
    public static class MailService
    {
        private const string Host = "smtp.gmail.com";
        private const int PortNr = 587;
        private const string EmailCoI = "info.cityofideas@gmail.com";
        private const string DisplayNameCoI = "City Of Ideas";
        private const string PwdCoI = "CoIMySweet16";
        private static readonly UsersManager UsersManager = new UsersManager();
        
        public static void SendErrorMail(string routeWhereExceptionOccurred, Exception exceptionThatOccurred, IIdentity userIdentity)
        {            
            using (SmtpClient client = new SmtpClient
            {
                Host = Host,
                Port = PortNr,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailCoI, PwdCoI),
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                using (MailMessage mail = new MailMessage {From = new MailAddress(EmailCoI, DisplayNameCoI)})
                {                    
                    mail.To.Add(EmailCoI);
                    foreach (var user in UsersManager.GetUsers("SUPERADMIN"))
                    {
                        mail.To.Add(new MailAddress(user.Email));
                    }

                    mail.Subject = "Error";
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    mail.Body = $@"
        <html>
        <body>
            <h1>An Error Has Occurred!</h1>
            <table cellpadding=""5"" cellspacing=""0"" border=""1"">
                <tr>
                    <td style=""text-align: right;font-weight: bold"">URL:</td>
                    <td>{routeWhereExceptionOccurred}</td>
                </tr>
                <tr>
                    <td style=""text-align: right;font-weight: bold"">User:</td>
                    <td>{userIdentity.Name}</td>
                </tr>
                <tr>
                    <td style=""text-align: right;font-weight: bold"">Exception Type:</td>
                    <td>{exceptionThatOccurred.GetType().Name}</td>
                </tr>
                <tr>
                    <td style=""text-align: right;font-weight: bold"">Message:</td>
                    <td>{exceptionThatOccurred}</td>
                </tr>
                <tr>
                    <td style=""text-align: right;font-weight: bold"">Stack Trace:</td>
                    <td>{exceptionThatOccurred.Message.Replace(Environment.NewLine, "<br />")}</td>
                </tr>
            </table>
        </body>
        </html>";

                    client.Send(mail);
                }
            }
        }
    }
}