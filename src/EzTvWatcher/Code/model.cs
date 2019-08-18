using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EzTvWatcher.Code
{
    public class RssItem
    {

        public string title { get; set; }

        public string guid { get; set; }

        public DateTimeOffset pubDate { get; set; }

        public bool IsOfInterest(List<String> interestList, DateTimeOffset dt)
        {
            bool result = false;
            if (this.pubDate > dt)
            {
                foreach (string interest in interestList)
                {
                    // case insensitive check to eliminate user input case differences
                    string invariantText = this.title.ToUpperInvariant();
                    string[] words = interest.Split(',');
                    bool matches = words.All(kw => invariantText.Contains(kw.ToUpperInvariant()));
                    result = matches || result;
                    if (result)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public string GetMailBody()
        {
            return $"<a href='{this.guid}'>{this.title}</a><br/>";
        }

    }

    public class GoogleApiConfig
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }

        public string SpreadsheetId { get; set; }

        public string ApplicationName { get; set; }
        public string Sheet { get; set; }
    }




}
