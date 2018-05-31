using Authentication.Filters;
using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Controllers
{
    public enum InspectorsSortState
    {
        FullNameAsc,
        FullNameDesc,
        SubdivisionAsc,
        SubdivisionDesc
    }

    [ServiceFilter(typeof(MethodsInvocationLoggingAttribute))]
    [Authorize]
    public class InspectorsController : Controller
    {
        const int pageSize = 10;

        readonly CarServiceContext _context;

        public InspectorsController(CarServiceContext context)
        {
            _context = context;
        }

        [InspectorFormStateSaving]
        public IActionResult Index(int page = 1, InspectorsSortState sortOrder = InspectorsSortState.FullNameAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Inspector> inspectors, "Inspectors"))
            {
                inspectors = _context.Inspectors.ToList();

                HttpContext.Session.Set(inspectors, "Inspectors");
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

            var count = inspectors.Count();
            inspectors = inspectors.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(inspectors);
        }

        [HttpPost]
        [InspectorFormStateSaving]
        public IActionResult Index(InspectorFilter filter, int page = 1, InspectorsSortState sortOrder = InspectorsSortState.FullNameAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Inspector> inspectors, "Inspectors"))
            {
                inspectors = _context.Inspectors.ToList();

                HttpContext.Session.Set(inspectors, "Inspectors");
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

            var count = inspectors.Count();
            inspectors = inspectors.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(inspectors);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create(Inspector model)
        {
            if (ModelState.IsValid)
            {
                _context.Inspectors.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(int id)
        {
            var model = _context.Inspectors.FirstOrDefault(i => i.InspectorId == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(Inspector model)
        {
            if (ModelState.IsValid)
            {
                var inspector = _context.Inspectors.FirstOrDefault(i => i.InspectorId == model.InspectorId);

                if (inspector != null)
                {
                    inspector.FullName = model.FullName;
                    inspector.Subdivision = model.Subdivision;

                    _context.Inspectors.Update(inspector);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _context.Inspectors.FirstOrDefault(i => i.InspectorId == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Delete(int id)
        {
            var model = _context.Inspectors.FirstOrDefault(i => i.InspectorId == id);

            if (model != null)
            {
                _context.Inspectors.Remove(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
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
