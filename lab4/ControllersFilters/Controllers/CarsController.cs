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
    public enum CarsSortState
    {
        StateNumberAsc,
        StateNumberDesc,
        MarkAsc,
        MarkDesc,
        ReleaseYearAsc,
        ReleaseYearDesc
    }

    public class CarsController : Controller
    {
        readonly CarServiceContext _context;

        public CarsController(CarServiceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(CarFilter filter,
            CarsSortState sortOrder = CarsSortState.StateNumberAsc)
        {
            var cars = _context.Cars.AsQueryable();

            ViewData["StateNumber"] = sortOrder == CarsSortState.StateNumberAsc ?
                CarsSortState.StateNumberDesc : CarsSortState.StateNumberAsc;
            ViewData["Mark"] = sortOrder == CarsSortState.MarkAsc ?
                CarsSortState.MarkDesc : CarsSortState.MarkAsc;
            ViewData["ReleaseYear"] = sortOrder == CarsSortState.ReleaseYearAsc ?
                CarsSortState.ReleaseYearDesc : CarsSortState.ReleaseYearAsc;

            switch (sortOrder)
            {
                case CarsSortState.StateNumberAsc:
                    cars = cars.OrderBy(c => c.StateNumber);
                    break;
                case CarsSortState.StateNumberDesc:
                    cars = cars.OrderByDescending(c => c.StateNumber);
                    break;
                case CarsSortState.MarkAsc:
                    cars = cars.OrderBy(c => c.Mark);
                    break;
                case CarsSortState.MarkDesc:
                    cars = cars.OrderByDescending(c => c.Mark);
                    break;
                case CarsSortState.ReleaseYearAsc:
                    cars = cars.OrderBy(c => c.ReleaseYear);
                    break;
                case CarsSortState.ReleaseYearDesc:
                    cars = cars.OrderByDescending(c => c.ReleaseYear);
                    break;
                default:
                    break;
            }

            cars = cars.Where(c => c.StateNumber.StartsWith(filter.StateNumber ?? ""));
            cars = cars.Where(c => c.Mark.StartsWith(filter.Mark ?? ""));
            cars = cars.Where(c => filter.ReleaseYear != 0 ? c.ReleaseYear == filter.ReleaseYear : true);

            return View(await cars.AsNoTracking().ToListAsync());
        }
    }
}
