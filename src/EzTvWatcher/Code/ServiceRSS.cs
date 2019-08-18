using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;

namespace EzTvWatcher.Code
{
   
    public class ServiceRSS
    {

        private readonly string _FeedUri;
        public ServiceRSS(string feedUri)
        {

            if (!string.IsNullOrEmpty(feedUri))
            {
                _FeedUri = feedUri;
            }
        }

        public async Task<List<RssItem>> GetNewsFeed()
        {
            var rssNewsItems = new List<RssItem>();
            using (var xmlReader = XmlReader.Create(_FeedUri, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == Microsoft.SyndicationFeed.SyndicationElementType.Item)
                    {
                        ISyndicationItem item = await feedReader.ReadItem();
                        
                        rssNewsItems.Add(item.ConvertToNewsItem());
                    }
                }
            }
            return rssNewsItems;
        }
    }

    //Extension Methods for converting a ISyndicationItem to a NewsItem
    public static class SyndicationExtensions
    {
        public static RssItem ConvertToNewsItem(this ISyndicationItem item)
        {
            return new RssItem() { title = item.Title, pubDate = item.Published,guid = item.Id };
        }
    }

    //String extension methods for converting HtmlToPlainTest
    public static class StringExtensions
    {
        public static string PlainTextTruncate(this string input, int length)
        {
            string text = HtmlToPlainText(input);
            if (text.Length < length)
            {
                return text;
            }

            char[] terminators = { '.', ',', ';', ':', '?', '!' };
            int end = text.LastIndexOfAny(terminators, length);
            if (end == -1)
            {
                end = text.LastIndexOf(" ", length);
                return text.Substring(0, end) + "...";
            }
            return text.Substring(0, end + 1);
        }

       
        public static string HtmlToPlainText(this string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}
