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
        readonly ISession _session;

        public HomeController(CarServiceContext context, IMemoryCache cache, ISession session)
        {
            _context = context;
            _cache = cache;
            _session = session;
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

            var model = new CarsViewModel
            {
                Collection = new List<Car> { car },
                SelectListItems = new List<SelectListItem>
                {
                    new SelectListItem { Value = car.Mark, Text = car.Mark, Selected = car.Mark == mark }
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cars(Car car)
        {
            Response.Cookies.Append("StateNumber", car.StateNumber);
            Response.Cookies.Append("Mark", car.Mark);

            var model = new CarsViewModel
            {
                Collection = await _context
                    .Cars
                    .Where(c => c.StateNumber.StartsWith(car.StateNumber ?? "") && c.Mark == car.Mark)
                    .ToListAsync(),
                SelectListItems = await _context
                    .Cars
                    .Select(c => new SelectListItem { Value = c.Mark, Text = c.Mark })
                    .Distinct()
                    .ToListAsync()
            };

            return View(model);
        }
        
        public IActionResult Inspectors()
        {
            var inspector = _cache.Get<Inspector>("InspectorsTuple");

            var formData = _session.GetInspector("InspectorFormData");

            ViewData["FullName"] = formData.FullName;
            var subdivision = formData.Subdivision;

            var model = new InspectorsViewModel
            {
                Collection = new List<Inspector> { inspector },
                SelectListItems = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = inspector.Subdivision,
                        Text = inspector.Subdivision,
                        Selected = inspector.Subdivision == subdivision
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Inspectors(Inspector inspector)
        {
            var model = new InspectorsViewModel
            {
                Collection = await _context
                    .Inspectors
                    .Where(i => i.FullName.StartsWith(inspector.FullName ?? "") && i.Subdivision == inspector.Subdivision)
                    .ToListAsync(),
                SelectListItems = await _context
                    .Inspectors
                    .Select(i => new SelectListItem { Value = i.Subdivision, Text = i.Subdivision })
                    .Distinct()
                    .ToListAsync()
            };

            return View(model);
        }
        
        public IActionResult CarTechStates()
        {
            var state = _cache.Get<CarTechState>("StatesTuple");

            var formData = _session.GetCarTechState("StateFormData");

            ViewData["Suspension"] = formData.Suspension;
            var brakeSystem = formData.BrakeSystem;

            var model = new CarTechStatesViewModel
            {
                Collection = new List<CarTechState> { state },
                SelectListItems = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = state.BrakeSystem,
                        Text = state.BrakeSystem,
                        Selected = state.BrakeSystem == brakeSystem
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CarTechStates(CarTechState state)
        {
            var model = new CarTechStatesViewModel
            {
                Collection = await _context
                    .CarTechStates
                    .Include("Car")
                    .Include("Inspector")
                    .Where(s => s.Suspension.StartsWith(state.Suspension ?? "") && s.BrakeSystem == state.BrakeSystem)
                    .ToListAsync(),
                SelectListItems = await _context
                    .CarTechStates
                    .Select(s => new SelectListItem { Value = s.BrakeSystem, Text = s.BrakeSystem })
                    .Distinct()
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
