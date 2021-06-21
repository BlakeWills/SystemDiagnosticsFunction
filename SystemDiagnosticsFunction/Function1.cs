using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SystemDiagnosticsFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var activity = Startup.ActivitySource.StartActivity(nameof(Run));
            log.LogInformation("C# HTTP trigger function processed a request. IsActivityNull={isActivityNull} ActivityId={activityId}", Activity.Current == null, Activity.Current.TraceId);

            string responseMessage = $"Activity.Current.TraceId == {Activity.Current?.TraceId.ToString()}";
            return new OkObjectResult(responseMessage);
        }
    }
}
