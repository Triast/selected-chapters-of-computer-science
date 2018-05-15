using Authentication.Filters;
using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
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
    [Authorize]
    public class CarsController : Controller
    {
        const int pageSize = 10;

        readonly CarServiceContext _context;

        public CarsController(CarServiceContext context)
        {
            _context = context;
        }

        [CarFormStateSaving]
        public IActionResult Index(int page = 1, CarsSortState sortOrder = CarsSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Car> cars, "Cars"))
            {
                cars = _context.Cars.AsQueryable();

                HttpContext.Session.Set(cars, "Cars");
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

            var count = cars.Count();
            cars = cars.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(cars);
        }

        [HttpPost]
        [CarFormStateSaving]
        public IActionResult Index(CarFilter filter, int page = 1, CarsSortState sortOrder = CarsSortState.StateNumberAsc)
        {
            if (!HttpContext.Session.TryGet(out IEnumerable<Car> cars, "Cars"))
            {
                cars = _context.Cars.AsQueryable();

                HttpContext.Session.Set(cars, "Cars");
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

            var count = cars.Count();
            cars = cars.Skip((page - 1) * pageSize).Take(pageSize);

            ViewData["PageViewModel"] = new PageViewModel(count, page, pageSize);

            return View(cars);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create(Car model)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(int id)
        {
            var model = _context.Cars.FirstOrDefault(c => c.CarId == id);

            if (model == null)
            {
                return NotFound();
            }
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(Car model)
        {
            if (ModelState.IsValid)
            {
                var car = _context.Cars.FirstOrDefault(c => c.CarId == model.CarId);

                if (car != null)
                {
                    car.StateNumber = model.StateNumber;
                    car.TechnicalPassport = model.TechnicalPassport;
                    car.Mark = model.Mark;
                    car.EngineVolume = model.EngineVolume;
                    car.BodyNumber = model.BodyNumber;
                    car.EngineNumber = model.EngineNumber;
                    car.ReleaseYear = model.ReleaseYear;
                    car.OwnerName = model.OwnerName;

                    _context.Cars.Update(car);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _context.Cars.FirstOrDefault(c => c.CarId == id);

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
            var model = _context.Cars.FirstOrDefault(c => c.CarId == id);

            if (model != null)
            {
                _context.Cars.Remove(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
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
