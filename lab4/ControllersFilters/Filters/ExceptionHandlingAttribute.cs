using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ControllersFilters.Filters
{
    public class ExceptionHandlingAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult { Content = context.Exception.Message, StatusCode = 404 };
        }
    }
}
