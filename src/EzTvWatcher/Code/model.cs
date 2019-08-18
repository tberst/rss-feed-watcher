using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EzTvWatcher.Code
{
  


   
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false,ElementName ="item")]
    public class RssItem
    {

        private string titleField;

        private string categoryField;

        private string linkField;

        private string guidField;

        private string pubDateField;

        private uint contentLengthField;

        private string infoHashField;

        private string magnetURIField;

        private byte seedsField;

        private byte peersField;

        private byte verifiedField;

        private string fileNameField;

    

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        public string guid
        {
            get
            {
                return this.guidField;
            }
            set
            {
                this.guidField = value;
            }
        }

        /// <remarks/>
        public string pubDate
        {
            get
            {
                return this.pubDateField;
            }
            set
            {
                this.pubDateField = value;
            }
        }

        public DateTimeOffset pubDateAsDt
        {
            get
            {
                DateTimeOffset dt;
                if (DateTimeOffset.TryParse(this.pubDate,out dt))
                {
                    return dt;
                }
                return DateTimeOffset.MinValue;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public uint contentLength
        {
            get
            {
                return this.contentLengthField;
            }
            set
            {
                this.contentLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public string infoHash
        {
            get
            {
                return this.infoHashField;
            }
            set
            {
                this.infoHashField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public string magnetURI
        {
            get
            {
                return this.magnetURIField;
            }
            set
            {
                this.magnetURIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public byte seeds
        {
            get
            {
                return this.seedsField;
            }
            set
            {
                this.seedsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public byte peers
        {
            get
            {
                return this.peersField;
            }
            set
            {
                this.peersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public byte verified
        {
            get
            {
                return this.verifiedField;
            }
            set
            {
                this.verifiedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.ezrss.it/0.1/")]
        public string fileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        public bool IsOfInterest(List<String> interestList, DateTimeOffset dt)
        {
            bool result = false;
            if (this.pubDateAsDt > dt)
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
            return $"<a href='{this.guid}'>{this.title}</a> - <a href='{this.magnetURI}'>MAGNET</a><br/>";
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
