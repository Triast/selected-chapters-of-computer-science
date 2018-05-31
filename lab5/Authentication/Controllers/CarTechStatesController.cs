using Authentication.Filters;
using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Controllers
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
    [Authorize]
    public class CarTechStatesController : Controller
    {
        const int pageSize = 10;

        readonly CarServiceContext _context;

        public CarTechStatesController(CarServiceContext context)
        {
            _context = context;
        }
        
        [CarTechStateFormStateSaving]
        public IActionResult Index(int page = 1, CarTechStateSortState sortOrder = CarTechStateSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<CarTechState> states, "States"))
            {
                states = _context.CarTechStates.Include("Car").Include("Inspector").ToList();

                HttpContext.Session.Set(states, "States");
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
            
            var count = states.Count();
            states = states.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(states);
        }

        [HttpPost]
        [CarTechStateFormStateSaving]
        public IActionResult Index(StateFilter filter, int page = 1, CarTechStateSortState sortOrder = CarTechStateSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<CarTechState> states, "States"))
            {
                states = _context.CarTechStates.Include("Car").Include("Inspector").ToList();

                HttpContext.Session.Set(states, "States");
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

            var count = states.Count();
            states = states.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(states);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            ViewData["StateNumbers"] = _context.Cars
                .Select(c => new SelectListItem
                {
                    Value = c.CarId.ToString(),
                    Text = c.StateNumber
                })
                .OrderBy(i => i.Text)
                .ToList();
            ViewData["FullNames"] = _context.Inspectors
                .Select(i => new SelectListItem
                {
                    Value = i.InspectorId.ToString(),
                    Text = i.FullName
                })
                .OrderBy(i => i.Text)
                .ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create(CarTechState model)
        {
            if (ModelState.IsValid)
            {
                _context.CarTechStates.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(int id)
        {
            var model = _context.CarTechStates.FirstOrDefault(s => s.CarTechStateId == id);

            ViewData["StateNumbers"] = _context.Cars
                .Select(c => new SelectListItem
                {
                    Value = c.CarId.ToString(),
                    Text = c.StateNumber,
                    Selected = c.CarId == model.CarId
                })
                .OrderBy(i => i.Text)
                .ToList();
            ViewData["FullNames"] = _context.Inspectors
                .Select(i => new SelectListItem
                {
                    Value = i.InspectorId.ToString(),
                    Text = i.FullName,
                    Selected = i.InspectorId == model.InspectorId
                })
                .OrderBy(i => i.Text)
                .ToList();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(CarTechState model)
        {
            if (ModelState.IsValid)
            {
                var state = _context.CarTechStates.FirstOrDefault(c => c.CarTechStateId == model.CarTechStateId);

                if (state != null)
                {
                    state.CarId = model.CarId;
                    state.InspectorId = model.InspectorId;
                    state.Date = model.Date;
                    state.Mileage = model.Mileage;
                    state.BrakeSystem = model.BrakeSystem;
                    state.Suspension = model.Suspension;
                    state.Wheels = model.Wheels;
                    state.Lightning = model.Lightning;
                    state.AdditionalEquipment = model.AdditionalEquipment;
                    state.MarkOnPassageOfServiceStation = model.MarkOnPassageOfServiceStation;

                    _context.CarTechStates.Update(state);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _context
                .CarTechStates
                .Include("Car")
                .Include("Inspector")
                .FirstOrDefault(s => s.CarTechStateId == id);

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
            var model = _context.CarTechStates.FirstOrDefault(s => s.CarTechStateId == id);

            if (model != null)
            {
                _context.CarTechStates.Remove(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
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
