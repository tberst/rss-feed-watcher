using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTvWatcher.Code
{
    public interface IServiceMail
    {
        Task<bool> SendMail(string subject, string htmlContent, string plaintext);
    }

    public class ServiceSendGrid : IServiceMail
    {
        private readonly string _apiKey;
        private readonly string _mailTo;
        private readonly string _mailFrom = "noreply@eztvwatcher.me";


        public ServiceSendGrid(IServicePersistence persistence)
        {
            _apiKey = persistence.GetSendGridApiKey();
            _mailTo = persistence.GetEmailTo();
        }
        public async Task<bool> SendMail(string subject, string htmlContent, string plaintext)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_mailFrom, _mailFrom);
            var to = new EmailAddress(_mailTo, _mailTo);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plaintext, htmlContent);
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }
            else
            {
                string error = await response.Body.ReadAsStringAsync();
                return false;
            }
        }
    }
}
