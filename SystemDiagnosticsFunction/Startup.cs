using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using SystemDiagnosticsFunction;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SystemDiagnosticsFunction
{
    public class Startup : FunctionsStartup
    {
        public static readonly string ActivityName = Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly ActivitySource ActivitySource = new ActivitySource(ActivityName);

        private static ActivityListener _listener;
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(logBuilder => logBuilder.AddConsole());

            RegisterListener();
        }

        private void RegisterListener()
        {
            _listener = new ActivityListener
            {
                ShouldListenTo = ShouldListen,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
                ActivityStarted = activity => Debug.WriteLine($"{activity.OperationName} started"),
                ActivityStopped = activity => Debug.WriteLine($"{activity.OperationName} stopped"),
            };

            ActivitySource.AddActivityListener(_listener);
        }

        private bool ShouldListen(ActivitySource source)
        {
            var shouldListen = source.Name == ActivitySource.Name;
            Debug.WriteLine($"{nameof(ShouldListen)} called for {source.Name}. ShouldListen=={shouldListen}");
            return shouldListen;
        }
    }
}