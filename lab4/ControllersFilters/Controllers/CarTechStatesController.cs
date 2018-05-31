using ControllersFilters.Filters;
using ControllersFilters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ControllersFilters.Controllers
{
    public enum CarTechStateSortState
    {
        StateNumberAsc,
        StateNumberDesc,
        FullNameAsc,
        FullNameDesc,
        BrakeSystemAsc,
        BrakeSystemDesc
    }

    [ServiceFilter(typeof(MethodsInvocationLoggingAttribute))]
    public class CarTechStatesController : Controller
    {
        readonly CarServiceContext _context;

        public CarTechStatesController(CarServiceContext context)
        {
            _context = context;
        }
        
        [CarTechStateFormStateSaving]
        public IActionResult Index(CarTechStateSortState sortOrder = CarTechStateSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<CarTechState> states))
            {
                states = _context.CarTechStates.Include("Car").Include("Inspector").ToList();

                HttpContext.Session.Set(states);
            }

            states = SortStates(states, sortOrder);

            ViewData["StateNumbers"] = states
                .Select(s => new SelectListItem
                {
                    Value = s.Car.StateNumber,
                    Text = s.Car.StateNumber,
                    Selected = s.Car.StateNumber == ((string)HttpContext.Items["StateNumber"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();
            ViewData["FullNames"] = states
                .Select(s => new SelectListItem
                {
                    Value = s.Inspector.FullName,
                    Text = s.Inspector.FullName,
                    Selected = s.Inspector.FullName == ((string)HttpContext.Items["FullName"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            return View(states);
        }

        [HttpPost]
        [CarTechStateFormStateSaving]
        public IActionResult Index(StateFilter filter, CarTechStateSortState sortOrder = CarTechStateSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<CarTechState> states))
            {
                states = _context.CarTechStates.Include("Car").Include("Inspector").ToList();

                HttpContext.Session.Set(states);
            }

            states = SortStates(states, sortOrder);

            states = states
                .Where(s => s.Car.StateNumber.StartsWith(filter.StateNumber ?? ""))
                .Where(s => s.Inspector.FullName.StartsWith(filter.FullName ?? ""))
                .Where(s => s.BrakeSystem.StartsWith(filter.BrakeSystem ?? ""));

            ViewData["StateNumbers"] = states
                .Select(s => new SelectListItem
                {
                    Value = s.Car.StateNumber,
                    Text = s.Car.StateNumber,
                    Selected = s.Car.StateNumber == ((string)HttpContext.Items["StateNumber"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();
            ViewData["FullNames"] = states
                .Select(s => new SelectListItem
                {
                    Value = s.Inspector.FullName,
                    Text = s.Inspector.FullName,
                    Selected = s.Inspector.FullName == ((string)HttpContext.Items["FullName"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            return View(states);
        }

        IEnumerable<CarTechState> SortStates(IEnumerable<CarTechState> states, CarTechStateSortState sortOrder)
        {
            ViewData["StateNumber"] = sortOrder == CarTechStateSortState.StateNumberAsc ?
                CarTechStateSortState.StateNumberDesc : CarTechStateSortState.StateNumberAsc;
            ViewData["FullName"] = sortOrder == CarTechStateSortState.FullNameAsc ?
                CarTechStateSortState.FullNameDesc : CarTechStateSortState.FullNameAsc;
            ViewData["BrakeSystem"] = sortOrder == CarTechStateSortState.BrakeSystemAsc ?
                CarTechStateSortState.BrakeSystemDesc : CarTechStateSortState.BrakeSystemAsc;

            switch (sortOrder)
            {
                case CarTechStateSortState.StateNumberAsc:
                    return states.OrderBy(c => c.Car.StateNumber);
                case CarTechStateSortState.StateNumberDesc:
                    return states.OrderByDescending(c => c.Car.StateNumber);
                case CarTechStateSortState.FullNameAsc:
                    return states.OrderBy(c => c.Inspector.FullName);
                case CarTechStateSortState.FullNameDesc:
                    return states.OrderByDescending(c => c.Inspector.FullName);
                case CarTechStateSortState.BrakeSystemAsc:
                    return states.OrderBy(c => c.BrakeSystem);
                case CarTechStateSortState.BrakeSystemDesc:
                    return states.OrderByDescending(c => c.BrakeSystem);
            }

            return null;
        }
    }
}
