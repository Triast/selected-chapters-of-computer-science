using ControllersFilters.Filters;
using ControllersFilters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ControllersFilters.Controllers
{
    public enum InspectorsSortState
    {
        FullNameAsc,
        FullNameDesc,
        SubdivisionAsc,
        SubdivisionDesc
    }

    [ServiceFilter(typeof(MethodsInvocationLoggingAttribute))]
    public class InspectorsController : Controller
    {
        readonly CarServiceContext _context;

        public InspectorsController(CarServiceContext context)
        {
            _context = context;
        }

        [InspectorFormStateSaving]
        public IActionResult Index(InspectorsSortState sortOrder = InspectorsSortState.FullNameAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Inspector> inspectors))
            {
                inspectors = _context.Inspectors.ToList();

                HttpContext.Session.Set(inspectors);
            }

            ViewData["Subdivisions"] = inspectors
                .Select(i => new SelectListItem
                {
                    Value = i.Subdivision,
                    Text = i.Subdivision,
                    Selected = i.Subdivision == ((string)HttpContext.Items["Subdivision"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            inspectors = SortInspectors(inspectors, sortOrder);

            return View(inspectors);
        }

        [HttpPost]
        [InspectorFormStateSaving]
        public IActionResult Index(InspectorFilter filter, InspectorsSortState sortOrder = InspectorsSortState.FullNameAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Inspector> inspectors))
            {
                inspectors = _context.Inspectors.ToList();

                HttpContext.Session.Set(inspectors);
            }

            ViewData["Subdivisions"] = inspectors
                .Select(i => new SelectListItem
                {
                    Value = i.Subdivision,
                    Text = i.Subdivision,
                    Selected = i.Subdivision == ((string)HttpContext.Items["Subdivision"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            inspectors = SortInspectors(inspectors, sortOrder);

            inspectors = inspectors.Where(i => i.FullName.StartsWith(filter.FullName ?? ""));
            inspectors = inspectors.Where(i => i.Subdivision.StartsWith(filter.Subdivision ?? ""));

            return View(inspectors);
        }

        IEnumerable<Inspector> SortInspectors(IEnumerable<Inspector> inspectors, InspectorsSortState sortOrder)
        {
            ViewData["FullName"] = sortOrder == InspectorsSortState.FullNameAsc ?
                InspectorsSortState.FullNameDesc : InspectorsSortState.FullNameAsc;
            ViewData["Subdivision"] = sortOrder == InspectorsSortState.SubdivisionAsc ?
                InspectorsSortState.SubdivisionDesc : InspectorsSortState.SubdivisionAsc;

            switch (sortOrder)
            {
                case InspectorsSortState.FullNameAsc:
                    return inspectors.OrderBy(i => i.FullName);
                case InspectorsSortState.FullNameDesc:
                    return inspectors.OrderByDescending(i => i.FullName);
                case InspectorsSortState.SubdivisionAsc:
                    return inspectors.OrderBy(i => i.Subdivision);
                case InspectorsSortState.SubdivisionDesc:
                    return inspectors.OrderByDescending(i => i.Subdivision);
            }

            return null;
        }
    }
}
