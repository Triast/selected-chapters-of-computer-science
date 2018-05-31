using ControllersFilters.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ControllersFilters.Filters
{
    public class CarTechStateFormStateSavingAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            session.TryGet(out StateFilter filter);
            
            if (context.HttpContext.Request.HasFormContentType)
            {
                var form = context.HttpContext.Request.Form;

                filter = new StateFilter
                {
                    StateNumber = form["StateNumber"],
                    FullName = form["FullName"],
                    BrakeSystem = form["BrakeSystem"]
                };

                session.Set(filter);
            }

            if (filter != null)
            {
                context.HttpContext.Items["StateNumber"] = filter.StateNumber;
                context.HttpContext.Items["FullName"] = filter.FullName;
                context.HttpContext.Items["BrakeSystem"] = filter.BrakeSystem;
            }
        }
    }
}
