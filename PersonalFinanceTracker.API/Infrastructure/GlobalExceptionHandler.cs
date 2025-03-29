using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.API.Infrastructure
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var response = new ApiErrorResponse
            {
                Message = "An error occurred while processing your request.",
                ExceptionMessage = context.Exception.Message,
                ExceptionType = context.Exception.GetType().Name,
                StackTrace = context.Exception.StackTrace
            };

            // Log the exception
            // TODO: Implement proper logging mechanism
            System.Diagnostics.Debug.WriteLine($"Exception: {context.Exception}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {context.Exception.StackTrace}");

            var httpResponse = context.Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            context.Result = new ResponseMessageResult(httpResponse);
        }

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);
            return Task.FromResult(0);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
} 