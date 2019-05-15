using System;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Services
{
    public static class MailService
    {
        private static readonly UsersManager UsersManager = new UsersManager();

        public static void SendErrorMail(string email, string password, string routeWhereExceptionOccurred,
            Exception exceptionThatOccurred, IIdentity userIdentity)
        {
            using (SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            })
            {
                using (MailMessage mail = new MailMessage {From = new MailAddress(email, "CityOfIdeas")})
                {
                    mail.To.Add("info.cityofideas@gmail.com");
                    foreach (var user in UsersManager.GetUsers("SuperAdmin"))
                    {
                        mail.To.Add(user.Email);
                    }

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
                    <td>{exceptionThatOccurred.Message}</td>
                </tr>
                <tr>
                    <td style=""text-align: right;font-weight: bold"">Stack Trace:</td>
                    <td>{exceptionThatOccurred.StackTrace.Replace(Environment.NewLine, "<br />")}</td>
                </tr>
            </table>
        </body>
        </html>";

                    client.SendMailAsync(mail);
                }
            }
        }
    }
}