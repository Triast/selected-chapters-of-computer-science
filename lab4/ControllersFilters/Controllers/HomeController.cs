using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControllersFilters.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ControllersFilters.Controllers
{
    public class HomeController : Controller
    {
        readonly CarServiceContext _context;

        public HomeController(CarServiceContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task<IActionResult> Cars()
        {
            var model = new CarsViewModel
            {
                Collection = await _context
                    .Cars
                    .ToListAsync(),
                SelectListItems = await _context
                    .Cars
                    .Select(c => new SelectListItem { Value = c.Mark, Text = c.Mark })
                    .Distinct()
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cars(Car car)
        {
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
        
        public async Task<IActionResult> Inspectors()
        {
            var model = new InspectorsViewModel
            {
                Collection = await _context
                    .Inspectors
                    .ToListAsync(),
                SelectListItems = await _context
                    .Inspectors
                    .Select(i => new SelectListItem { Value = i.Subdivision, Text = i.Subdivision })
                    .Distinct()
                    .ToListAsync()
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
        
        public async Task<IActionResult> CarTechStates()
        {
            var model = new CarTechStatesViewModel
            {
                Collection = await _context
                    .CarTechStates
                    .Include("Car")
                    .Include("Inspector")
                    .ToListAsync(),
                SelectListItems = await _context
                    .CarTechStates
                    .Select(s => new SelectListItem { Value = s.BrakeSystem, Text = s.BrakeSystem })
                    .Distinct()
                    .ToListAsync()
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
