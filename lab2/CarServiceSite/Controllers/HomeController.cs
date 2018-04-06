using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarServiceSite.Models;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceSite.Controllers
{
    public class HomeController : Controller
    {
        static Car[] cars = new Car[]
        {
            new Car
            {
                StateNumber = "0123AA-01",
                TechnicalPassport = "AA123456",
                Mark = "Lada",
                EngineVolume = 500,
                BodyNumber = "AAAA0123",
                EngineNumber = "ASCX4513",
                ReleaseYear = 1990,
                OwnerName = "Комарец Игорь Валерьевич"
            },
            new Car
            {
                StateNumber = "1225FA-01",
                TechnicalPassport = "AS215463",
                Mark = "VAZ",
                EngineVolume = 600,
                BodyNumber = "ABDF6514",
                EngineNumber = "FASC5412",
                ReleaseYear = 2000,
                OwnerName = "Комарец Виктор Валерьевич"
            }
        };
        static List<SelectListItem> marks = new List<SelectListItem>
        {
            new SelectListItem() {Value = "Lada", Text = "Lada"},
            new SelectListItem() {Value = "VAZ", Text = "VAZ"}
        };

        static Inspector[] inspectors = new Inspector[]
        {
            new Inspector
            {
                FullName = "Иванов Иван Иванович",
                Subdivision = "Подразделение_1"
            },
            new Inspector
            {
                FullName = "Петров Пётр Петрович",
                Subdivision = "Подразделение_2"
            }
        };
        static List<SelectListItem> subdivisions = new List<SelectListItem>
        {
            new SelectListItem { Value = "Подразделение_1", Text = "Подразделение_1" },
            new SelectListItem { Value = "Подразделение_2", Text = "Подразделение_2" }
        };

        static CarTechState[] states = new CarTechState[]
        {
            new CarTechState
            {
                CarId = 1,
                InspectorId = 2,
                Date = DateTime.Now,
                Mileage = 500,
                BrakeSystem = "Тормозная_система_1",
                Suspension = "Подвеска_1",
                Wheels = "Колёса_1",
                Lightning = "Осветительный_прибор_1",
                AdditionalEquipment = "Отсутствует",
                MarkOnPassageOfServiceStation = true,

                Car = cars[0],
                Inspector = inspectors[1]
            },
            new CarTechState
            {
                CarId = 2,
                InspectorId = 1,
                Date = DateTime.Now.AddMonths(-2),
                Mileage = 600,
                BrakeSystem = "Тормозная_система_2",
                Suspension = "Подвеска_2",
                Wheels = "Колёса_2",
                Lightning = "Осветительный_прибор_2",
                AdditionalEquipment = "Отсутствует",
                MarkOnPassageOfServiceStation = false,

                Car = cars[1],
                Inspector = inspectors[0]
            }
        };
        static List<SelectListItem> brakeSystems = new List<SelectListItem>
        {
            new SelectListItem { Value = "Тормозная_система_1", Text = "Тормозная_система_1" },
            new SelectListItem { Value = "Тормозная_система_2", Text = "Тормозная_система_2" }
        };

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

        public IActionResult Cars()
        {
            var model = new CarsViewModel
            {
                Collection = cars,
                SelectListItems = marks
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Cars(Car car)
        {
            var model = new CarsViewModel
            {
                SelectListItems = marks
            };
            
            model.Collection = cars.Where(c => c.StateNumber.StartsWith(car.StateNumber ?? ""));
            model.Collection = model.Collection.Where(c => c.Mark == car.Mark);

            return View(model);
        }

        public IActionResult Inspectors()
        {
            var model = new InspectorsViewModel
            {
                Collection = inspectors,
                SelectListItems = subdivisions
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Inspectors(Inspector inspector)
        {
            var model = new InspectorsViewModel
            {
                SelectListItems = subdivisions
            };
            
            model.Collection = inspectors.Where(i => i.FullName.StartsWith(inspector.FullName ?? ""));
            model.Collection = model.Collection.Where(c => c.Subdivision == inspector.Subdivision);

            return View(model);
        }

        public IActionResult CarTechStates()
        {
            var model = new CarTechStatesViewModel
            {
                Collection = states,
                SelectListItems = brakeSystems
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CarTechStates(CarTechState state)
        {
            var model = new CarTechStatesViewModel
            {
                SelectListItems = brakeSystems
            };

            model.Collection = states.Where(s => s.Suspension.StartsWith(state.Suspension ?? ""));
            model.Collection = model.Collection.Where(s => s.BrakeSystem == state.BrakeSystem);

            return View(model);
        }
    }
}
