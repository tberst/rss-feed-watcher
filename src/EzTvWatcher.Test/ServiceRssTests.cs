using EzTvWatcher.Code;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EzTvWatcher.Test
{ 
    public class ServiceRssTest
    {
        private ServiceRSS _service;

        public ServiceRssTest()
        {
            this._service = new ServiceRSS("https://eztv.io/ezrss.xml");
        }
        [SetUp]
        public void Setup()
        {
            

        }

        [Test]
        public async Task Test1()
        {
            var result = await this._service.GetNewsFeed();
            Assert.IsTrue(result.Count > 0);
        }
    }
}