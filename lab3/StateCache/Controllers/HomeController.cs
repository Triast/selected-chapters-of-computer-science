using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StateCache.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StateCache.Controllers
{
    public class HomeController : Controller
    {
        readonly CarServiceContext _context;
        readonly IMemoryCache _cache;

        public HomeController(CarServiceContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult Cars()
        {
            var car = _cache.Get<Car>("CarsTuple");

            ViewData["StateNumber"] = Request.Cookies["StateNumber"];
            var mark = Request.Cookies["Mark"];

            ViewData["Marks"] = new List<SelectListItem>
            {
                new SelectListItem { Value = car.Mark, Text = car.Mark, Selected = car.Mark == mark }
            };

            var model = new List<Car> { car };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cars(Car car)
        {
            Response.Cookies.Append("StateNumber", car.StateNumber);
            Response.Cookies.Append("Mark", car.Mark);

            ViewData["Marks"] = await _context
                .Cars
                .Select(c => new SelectListItem { Value = c.Mark, Text = c.Mark })
                .Distinct()
                .ToListAsync();

            var model = await _context
                .Cars
                .Where(c => c.StateNumber.StartsWith(car.StateNumber ?? "") && c.Mark == car.Mark)
                .ToListAsync();

            return View(model);
        }
        
        public IActionResult Inspectors()
        {
            var inspector = _cache.Get<Inspector>("InspectorsTuple");

            var formData = HttpContext.Session.GetInspector("InspectorFormData") ?? new Inspector();

            ViewData["FullName"] = formData.FullName;
            ViewData["Subdivisions"] = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = inspector.Subdivision,
                    Text = inspector.Subdivision,
                    Selected = inspector.Subdivision == formData.Subdivision
                }
            };

            var model = new List<Inspector> { inspector };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Inspectors(Inspector inspector)
        {
            HttpContext.Session.SetInspector("InspectorFormData", inspector);

            ViewData["Subdivisions"] = await _context
                .Inspectors
                .Select(i => new SelectListItem { Value = i.Subdivision, Text = i.Subdivision })
                .Distinct()
                .ToListAsync();

            var model = await _context
                .Inspectors
                .Where(i => i.FullName.StartsWith(inspector.FullName ?? ""))
                .Where(i => i.Subdivision == inspector.Subdivision)
                .ToListAsync();

            return View(model);
        }

        public IActionResult CarTechStates()
        {
            var state = _cache.Get<CarTechState>("StatesTuple");

            var formData = HttpContext.Session.GetState("StateFormData");

            ViewData["Suspension"] = formData.Suspension;
            ViewData["BrakeSystems"] = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = state.BrakeSystem,
                    Text = state.BrakeSystem,
                    Selected = state.BrakeSystem == formData.BrakeSystem
                }
            };

            var model = new List<CarTechState> { state };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CarTechStates(StateFilter state)
        {
            HttpContext.Session.SetState("StateFormData", state);

            ViewData["BrakeSystems"] = await _context
                .CarTechStates
                .Select(s => new SelectListItem { Value = s.BrakeSystem, Text = s.BrakeSystem })
                .Distinct()
                .ToListAsync();

            var model = await _context
                .CarTechStates
                .Include("Car")
                .Include("Inspector")
                .Where(s => s.Suspension.StartsWith(state.Suspension ?? ""))
                .Where(s => s.BrakeSystem == state.BrakeSystem)
                .ToListAsync();

            return View(model);
        }
    }
}
