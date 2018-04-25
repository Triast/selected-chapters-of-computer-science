using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ControllersFilters.Filters
{
    public class MethodsInvocationLoggingAttribute : Attribute, IAsyncActionFilter
    {
        IHttpContextAccessor _accessor;
        ILogger _logger;

        public MethodsInvocationLoggingAttribute(ILoggerFactory factory, IHttpContextAccessor accessor)
        {
            _logger = factory.CreateLogger("MethodsInvocationLogging");
            _accessor = accessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ipAddr = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var actionName = context.ActionDescriptor.DisplayName;

            _logger.LogInformation($"{ipAddr} - {actionName} - {DateTime.Now}");

            await next();
        }
    }
}
