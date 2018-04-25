using ControllersFilters.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ControllersFilters.Filters
{
    public class CarFormStateSavingAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            session.TryGet(out CarFilter filter);
            
            if (context.HttpContext.Request.HasFormContentType)
            {
                var form = context.HttpContext.Request.Form;

                filter = new CarFilter
                {
                    StateNumber = form["StateNumber"],
                    Mark = form["Mark"],
                    ReleaseYear = int.Parse(form["ReleaseYear"])
                };

                session.Set(filter);
            }

            if (filter != null)
            {
                context.HttpContext.Items["StateNumber"] = filter.StateNumber;
                context.HttpContext.Items["Mark"] = filter.Mark;
                context.HttpContext.Items["ReleaseYear"] = filter.ReleaseYear;
            }
        }
    }
}
