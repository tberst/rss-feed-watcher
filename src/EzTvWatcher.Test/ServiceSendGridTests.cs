using EzTvWatcher.Code;
using EzTvWatcher.Test;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EzTvWatcher.Test
{ 
    public class ServiceSendGridTest
    {
        private ServiceSendGrid _service;

        public ServiceSendGridTest()
        {
           
            this._service = new ServiceSendGrid(new FakePersistence());
        }
        [SetUp]
        public void Setup()
        {
            

        }

        [Test]
        public async Task Test1()
        {
            string html = @"<a href='https://eztv.io/ep/1358820/the-kitchen-s22e02-fun-family-favorites-480p-x264-msd/'>The Kitchen S22E02 Fun Family Favorites 480p x264-mSD</a><br/>
<a href='https://eztv.io/ep/1358812/the-kitchen-s22e02-fun-family-favorites-web-x264-caffeine/'>The Kitchen S22E02 Fun Family Favorites WEB x264-CAFFEiNE</a><br/>";

            string plaintext = @"The Kitchen S22E02 Fun Family Favorites 480p x264-mSD
The Kitchen S22E02 Fun Family Favorites WEB x264-CAFFEiNE
";
            var result = await this._service.SendMail("EzTvWatcher",html,plaintext);
           
        }
    }
}