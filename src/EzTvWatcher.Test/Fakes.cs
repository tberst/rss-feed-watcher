using EzTvWatcher.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTvWatcher.Test
{
     class FakePersistence : IServicePersistence
    {
        public string GetEmailTo()
        {
            return "test@test.com";
        }

        public string GetSendGridApiKey()
        {
            return "1234";
        }

        public DateTimeOffset ReadLastProcessedDate()
        {
            return DateTimeOffset.Now.AddMinutes(-5);
        }

        public List<string> ReadWatchList()
        {
            return new List<string>() { "Champions" };
        }

        public void WriteLastProcessedDate(DateTimeOffset dt)
        {
            
        }
    }

    class FakeMail : IServiceMail
    {
        public  Task<bool> SendMail(string subject, string htmlContent, string plaintext)
        {
            return Task.FromResult(true);
        }
    }

}
