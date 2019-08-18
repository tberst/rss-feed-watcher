using EzTvWatcher.Code;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTvWatcher.Test
{
    public class RssItemTest
    {
        private RssItem _item;

        public RssItemTest()
        {
            this._item = new RssItem();
            _item.title = "The Kitchen S22E02 Fun Family Favorites 480p x264-mSD";
            _item.pubDate = DateTimeOffset.Now;
        }
        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void IsOfInterest()
        {
            var test1 = new List<string>() { "the,kitchen", "Diners" };
            Assert.IsTrue(this._item.IsOfInterest(test1, DateTimeOffset.Now.AddDays(-1)));

            var test2 = new List<string>() { "test,test", "Diners" };
            Assert.IsFalse(this._item.IsOfInterest(test2, DateTimeOffset.Now.AddDays(-1)));

            var test3 = new List<string>() { "the,kitchen", "Diners" };
            Assert.IsFalse(this._item.IsOfInterest(test1, DateTimeOffset.Now.AddDays(+1)));

        }

    }
}
