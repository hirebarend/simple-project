using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public static class EchoFunction
    {
        [FunctionName("Echo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = null)] HttpRequest httpRequest,
            ILogger logger)
        {
            logger.LogInformation(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));

            return new OkObjectResult(new
            {
                url = $"https://{Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")}.azurewebsites.net/api/Echo",
            });
        }
    }
}
