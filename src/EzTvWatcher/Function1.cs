using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

using EzTvWatcher.Code;

namespace EzTvWatcher
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
               .SetBasePath(context.FunctionAppDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            log.LogInformation("load google api config");
            var googleConfig = new GoogleApiConfig();
            config.GetSection("GoogleApiConfig").Bind(googleConfig);
            googleConfig.private_key = googleConfig.private_key.Replace(@"\n", "\n");

            var mylogger = new ServiceLogger();
            using (var google = new ServiceGoogleSheet(googleConfig, mylogger))
            {
                var rss = new ServiceRSS(google.GetRssFeedUrl());
                var sendgrid = new ServiceSendGrid(google);
                var processor = new Processor(google,rss,sendgrid ,mylogger );
                await processor.Run();
            }

            log.LogInformation(mylogger.Flush());

            return (ActionResult)new OkObjectResult(mylogger.Flush());

        }
    }
}
