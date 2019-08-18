using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTvWatcher.Code
{
   public  class Processor
    {
        private IServicePersistence _google;
        private ServiceRSS _rss;
        private IServiceMail _sendgrid;
        private ServiceLogger _logger;

        public Processor(IServicePersistence google,ServiceRSS rss, IServiceMail sendgrid,ServiceLogger logger)
        {
            this._google = google;
            this._rss = rss;
            this._sendgrid = sendgrid;
            this._logger = logger;
        }

        public async Task<bool> Run()
        {
            this._logger.Debug("RUN");
            var rssItems = await _rss.GetNewsFeed();
            var lastCheckedDate =  this._google.ReadLastProcessedDate();
            var listOfInterest = this._google.ReadWatchList();
            var maxCheckedDate = lastCheckedDate;
            StringBuilder mailbody = new StringBuilder() ;
            StringBuilder plaintext = new StringBuilder();
            this._logger.Debug($"found {rssItems.Count} items");
            int count = 0;
            foreach (var item in rssItems)
            {
                if (item.IsOfInterest(listOfInterest,lastCheckedDate))
                {
                    count++;
                    mailbody.AppendLine(item.GetMailBody());
                    plaintext.AppendLine(item.title);
                }
                if (maxCheckedDate < item.pubDateAsDt)
                {
                    maxCheckedDate = item.pubDateAsDt;
                }
            }
            _logger.Debug($"found {count} interesting items");
            this._google.WriteLastProcessedDate(maxCheckedDate);
            if (count>0)
            {
                return await this._sendgrid.SendMail("EzTvWatcher", mailbody.ToString(), plaintext.ToString());
            }
            return true;
        }
    }
}
