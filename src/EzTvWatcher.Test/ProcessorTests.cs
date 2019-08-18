using EzTvWatcher.Code;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTvWatcher.Test
{
    public class ProcessorTests
    {
        private Processor _processor;

        public ProcessorTests()
        {
            this._processor = new Processor(
                new FakePersistence(),
                new ServiceRSS("https://eztv.io/ezrss.xml"),
                new FakeMail(),
                new ServiceLogger()
                ) ;
        }
        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public async Task Run()
        {
            await this._processor.Run();

        }

    }
}
