using EzTvWatcher.Code;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EzTvWatcher.Test
{
    public class ServiceGoogleSheetTest
    {
        private ServiceGoogleSheet _service;

        public ServiceGoogleSheetTest()
        {
            var config = ConfigHelper.GetGoogleApplicationConfiguration(TestContext.CurrentContext.TestDirectory);

            this._service = new EzTvWatcher.Code.ServiceGoogleSheet(config,new ServiceLogger());
        }
        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void ReadLastProcessedDate()
        {
            var result = this._service.ReadLastProcessedDate();
            Assert.IsNotNull(result);
        }
       
        [Test]
        public void ReadWatchList()
        {
            
            var watchlist = this._service.ReadWatchList();
            Assert.IsTrue(watchlist.Count > 0);
        }

        [Test]
        public void GetEmailTo()
        {

            var result = this._service.GetEmailTo();
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSendGridApiKey()
        {

            var result = this._service.GetSendGridApiKey();
            Assert.IsNotNull(result);
        }

    }
}