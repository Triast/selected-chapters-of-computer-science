using Authentication.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Authentication.Filters
{
    public class InspectorFormStateSavingAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            session.TryGet(out InspectorFilter filter, "InspectorFilter");
                
            if (context.HttpContext.Request.HasFormContentType)
            {
                var form = context.HttpContext.Request.Form;

                filter = new InspectorFilter
                {
                    FullName = form["FullName"],
                    Subdivision = form["Subdivision"]
                };

                session.Set(filter, "InspectorFilter");
            }

            if (filter != null)
            {
                context.HttpContext.Items["FullName"] = filter.FullName;
                context.HttpContext.Items["Subdivision"] = filter.Subdivision;
            }
        }
    }
}
