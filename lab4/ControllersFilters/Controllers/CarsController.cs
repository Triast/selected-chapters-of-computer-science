using ControllersFilters.Filters;
using ControllersFilters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ControllersFilters.Controllers
{
    public enum CarsSortState
    {
        StateNumberAsc,
        StateNumberDesc,
        MarkAsc,
        MarkDesc,
        ReleaseYearAsc,
        ReleaseYearDesc
    }

    [ServiceFilter(typeof(MethodsInvocationLoggingAttribute))]
    public class CarsController : Controller
    {
        readonly CarServiceContext _context;

        public CarsController(CarServiceContext context)
        {
            _context = context;
        }

        [CarFormStateSaving]
        public IActionResult Index(CarsSortState sortOrder = CarsSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Car> cars))
            {
                cars = _context.Cars.AsQueryable();

                HttpContext.Session.Set(cars);
            }

            ViewData["Marks"] = cars
                .Select(c => new SelectListItem
                {
                    Value = c.Mark,
                    Text = c.Mark,
                    Selected = c.Mark == ((string)HttpContext.Items["Mark"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            cars = SortCars(cars, sortOrder);

            return View(cars);
        }

        [HttpPost]
        [CarFormStateSaving]
        public IActionResult Index(CarFilter filter, CarsSortState sortOrder = CarsSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Car> cars))
            {
                cars = _context.Cars.AsQueryable();

                HttpContext.Session.Set(cars);
            }

            ViewData["Marks"] = cars
                .Select(c => new SelectListItem
                {
                    Value = c.Mark,
                    Text = c.Mark,
                    Selected = c.Mark == ((string)HttpContext.Items["Mark"] ?? "")
                })
                .Distinct(new SelectListItemEqualityComparer())
                .ToList();

            cars = SortCars(cars, sortOrder);

            cars = cars
                .Where(c => c.StateNumber.StartsWith(filter.StateNumber ?? ""))
                .Where(c => c.Mark.StartsWith(filter.Mark ?? ""))
                .Where(c => filter.ReleaseYear != 0 ? c.ReleaseYear == filter.ReleaseYear : true);

            return View(cars);
        }

        IEnumerable<Car> SortCars(IEnumerable<Car> cars, CarsSortState sortOrder)
        {
            ViewData["StateNumber"] = sortOrder == CarsSortState.StateNumberAsc ?
                CarsSortState.StateNumberDesc : CarsSortState.StateNumberAsc;
            ViewData["Mark"] = sortOrder == CarsSortState.MarkAsc ?
                CarsSortState.MarkDesc : CarsSortState.MarkAsc;
            ViewData["ReleaseYear"] = sortOrder == CarsSortState.ReleaseYearAsc ?
                CarsSortState.ReleaseYearDesc : CarsSortState.ReleaseYearAsc;

            switch (sortOrder)
            {
                case CarsSortState.StateNumberAsc:
                    return cars.OrderBy(c => c.StateNumber);
                case CarsSortState.StateNumberDesc:
                    return cars.OrderByDescending(c => c.StateNumber);
                case CarsSortState.MarkAsc:
                    return cars.OrderBy(c => c.Mark);
                case CarsSortState.MarkDesc:
                    return cars.OrderByDescending(c => c.Mark);
                case CarsSortState.ReleaseYearAsc:
                    return cars.OrderBy(c => c.ReleaseYear);
                case CarsSortState.ReleaseYearDesc:
                    return cars.OrderByDescending(c => c.ReleaseYear);
            }

            return null;
        }
    }
}
