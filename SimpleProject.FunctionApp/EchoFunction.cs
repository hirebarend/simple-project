using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public static class EchoFunction
    {
        [FunctionName("EchoFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = null)] HttpRequest httpRequest,
            ILogger logger)
        {
            return new OkObjectResult(true);
        }
    }
}
