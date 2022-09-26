using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PersonService.API.Helpers
{
    /// <summary>
    /// Operation cancelled middleware
    /// </summary>
    public class OperationCancelledExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<OperationCancelledExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCancelledExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public OperationCancelledExceptionFilter(ILogger<OperationCancelledExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Exception handling middleware
        /// </summary>
        /// <param name="context">Exception context</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is TaskCanceledException)
            {
                _logger.LogInformation("Request was cancelled by the user.");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(499);
            }
        }
    }
}
