using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StateCache.Models;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace StateCache.Controllers
{
    public class HomeController : Controller
    {
        //static Car[] cars = new Car[]
        //{
        //    new Car
        //    {
        //        StateNumber = "0123AA-01",
        //        TechnicalPassport = "AA123456",
        //        Mark = "Lada",
        //        EngineVolume = 500,
        //        BodyNumber = "AAAA0123",
        //        EngineNumber = "ASCX4513",
        //        ReleaseYear = 1990,
        //        OwnerName = "Комарец Игорь Валерьевич"
        //    },
        //    new Car
        //    {
        //        StateNumber = "1225FA-01",
        //        TechnicalPassport = "AS215463",
        //        Mark = "VAZ",
        //        EngineVolume = 600,
        //        BodyNumber = "ABDF6514",
        //        EngineNumber = "FASC5412",
        //        ReleaseYear = 2000,
        //        OwnerName = "Комарец Виктор Валерьевич"
        //    }
        //};
        //static List<SelectListItem> marks = new List<SelectListItem>
        //{
        //    new SelectListItem() {Value = "Lada", Text = "Lada"},
        //    new SelectListItem() {Value = "VAZ", Text = "VAZ"}
        //};

        //static Inspector[] inspectors = new Inspector[]
        //{
        //    new Inspector
        //    {
        //        FullName = "Иванов Иван Иванович",
        //        Subdivision = "Подразделение_1"
        //    },
        //    new Inspector
        //    {
        //        FullName = "Петров Пётр Петрович",
        //        Subdivision = "Подразделение_2"
        //    }
        //};
        //static List<SelectListItem> subdivisions = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "Подразделение_1", Text = "Подразделение_1" },
        //    new SelectListItem { Value = "Подразделение_2", Text = "Подразделение_2" }
        //};

        //static CarTechState[] states = new CarTechState[]
        //{
        //    new CarTechState
        //    {
        //        CarId = 1,
        //        InspectorId = 2,
        //        Date = DateTime.Now,
        //        Mileage = 500,
        //        BrakeSystem = "Тормозная_система_1",
        //        Suspension = "Подвеска_1",
        //        Wheels = "Колёса_1",
        //        Lightning = "Осветительный_прибор_1",
        //        AdditionalEquipment = "Отсутствует",
        //        MarkOnPassageOfServiceStation = true,

        //        Car = cars[0],
        //        Inspector = inspectors[1]
        //    },
        //    new CarTechState
        //    {
        //        CarId = 2,
        //        InspectorId = 1,
        //        Date = DateTime.Now.AddMonths(-2),
        //        Mileage = 600,
        //        BrakeSystem = "Тормозная_система_2",
        //        Suspension = "Подвеска_2",
        //        Wheels = "Колёса_2",
        //        Lightning = "Осветительный_прибор_2",
        //        AdditionalEquipment = "Отсутствует",
        //        MarkOnPassageOfServiceStation = false,

        //        Car = cars[1],
        //        Inspector = inspectors[0]
        //    }
        //};
        //static List<SelectListItem> brakeSystems = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "Тормозная_система_1", Text = "Тормозная_система_1" },
        //    new SelectListItem { Value = "Тормозная_система_2", Text = "Тормозная_система_2" }
        //};

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
                Collection = await _context.Cars.ToListAsync(),
                SelectListItems = await _context
                    .Cars
                    .Select(c => new SelectListItem { Value = c.Mark, Text = c.Mark })
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
                    .ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Inspectors()
        {
            var model = new InspectorsViewModel
            {
                Collection = await _context.Inspectors.ToListAsync(),
                SelectListItems = await _context
                    .Inspectors
                    .Select(i => new SelectListItem { Value = i.Subdivision, Text = i.Subdivision })
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
                    .ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> CarTechStates()
        {
            var model = new CarTechStatesViewModel
            {
                Collection = await _context.CarTechStates.ToListAsync(),
                SelectListItems = await _context
                    .CarTechStates
                    .Select(s => new SelectListItem { Value = s.BrakeSystem, Text = s.BrakeSystem })
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
                    .Where(s => s.Suspension.StartsWith(state.Suspension ?? "") && s.BrakeSystem == state.BrakeSystem)
                    .ToListAsync(),
                SelectListItems = await _context
                    .CarTechStates
                    .Select(s => new SelectListItem { Value = s.BrakeSystem, Text = s.BrakeSystem })
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
