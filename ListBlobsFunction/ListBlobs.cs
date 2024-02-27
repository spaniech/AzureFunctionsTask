using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ListBlobsFunction
{
    public class ListBlobs(ILogger<ListBlobs> logger)
    {
        private readonly ILogger<ListBlobs> _logger = logger;

        [Function(nameof(ListBlobs))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
