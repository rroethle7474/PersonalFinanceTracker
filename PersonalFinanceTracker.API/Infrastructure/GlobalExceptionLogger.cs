using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace PersonalFinanceTracker.API.Infrastructure
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            // TODO: Implement proper logging mechanism (e.g., using NLog, Serilog, or log4net)
            System.Diagnostics.Debug.WriteLine($"Exception: {context.Exception}");
            System.Diagnostics.Debug.WriteLine($"Request: {context.Request}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {context.Exception.StackTrace}");
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Log(context);
            return Task.FromResult(0);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return true;
        }
    }
} 