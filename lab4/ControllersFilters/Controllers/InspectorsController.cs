using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ControllersFilters.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ControllersFilters.Controllers
{
    public class InspectorsController : Controller
    {
        public enum InspectorsSortState
        {
            FullNameAsc,
            FullNameDesc,
            SubdivisionAsc,
            SubdivisionDesc
        }

        readonly CarServiceContext _context;

        public InspectorsController(CarServiceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Inspector filter,
            InspectorsSortState sortOrder = InspectorsSortState.FullNameAsc)
        {
            var inspectors = _context.Inspectors.AsQueryable();

            ViewData["FullName"] = sortOrder == InspectorsSortState.FullNameAsc ?
                InspectorsSortState.FullNameDesc : InspectorsSortState.FullNameAsc;
            ViewData["Subdivision"] = sortOrder == InspectorsSortState.SubdivisionAsc ?
                InspectorsSortState.SubdivisionDesc : InspectorsSortState.SubdivisionAsc;

            switch (sortOrder)
            {
                case InspectorsSortState.FullNameAsc:
                    inspectors = inspectors.OrderBy(i => i.FullName);
                    break;
                case InspectorsSortState.FullNameDesc:
                    inspectors = inspectors.OrderByDescending(i => i.FullName);
                    break;
                case InspectorsSortState.SubdivisionAsc:
                    inspectors = inspectors.OrderBy(i => i.Subdivision);
                    break;
                case InspectorsSortState.SubdivisionDesc:
                    inspectors = inspectors.OrderByDescending(i => i.Subdivision);
                    break;
                default:
                    break;
            }

            inspectors = inspectors.Where(i => i.FullName.StartsWith(filter.FullName ?? ""));
            inspectors = inspectors.Where(i => i.Subdivision.StartsWith(filter.Subdivision ?? ""));

            return View(await inspectors.AsNoTracking().ToListAsync());
        }
    }
}
