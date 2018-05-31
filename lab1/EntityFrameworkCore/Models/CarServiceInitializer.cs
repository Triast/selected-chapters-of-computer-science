using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCore.Models
{
    public class CarServiceInitializer
    {
        public static void Initialize(CarServiceContext context)
        {
            context.Database.EnsureCreated();

            if (context.CarTechStates.Any())
            {
                return;
            }
            
            const int inspectorCount = 35;
            const int carCount = 100;
            const int carTechStateCount = 200;

            var rand = new Random();

            var inspectors = new List<Inspector>();
            for (int i = 0; i < inspectorCount; i++)
            {
                inspectors.Add(new Inspector
                {
                    FullName = new FullName().ToString(),
                    Subdivision = "Подразделение_" + rand.Next(1, 6)
                });
            }

            context.Inspectors.AddRange(inspectors);

            var marks = new List<string> { "Alfa Romeo", "Aston Martin", "Bentley", "Bugatti", "Citroen", "DS", "Ferrari", "Fiat", "Jaguar", "Lamborghini" };

            var cars = new List<Car>();
            for (int i = 0; i < carCount; i++)
            {
                cars.Add(new Car
                {
                    StateNumber = new StateNumber().ToString(),
                    TechnicalPassport = new TechnicalPassport().ToString(),
                    Mark = marks[rand.Next(marks.Count)],
                    EngineVolume = rand.Next(60, 601),
                    BodyNumber = new AlphaDigitNumber().ToString(),
                    EngineNumber = new AlphaDigitNumber().ToString(),
                    ReleaseYear = rand.Next(1900, DateTime.Now.Year),
                    OwnerName = new FullName().ToString()
                });
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            int firstInspectorId = context.Inspectors.First().InspectorId;
            int firstCarId = context.Cars.First().CarId;

            var carTechStates = new List<CarTechState>();
            for (int i = 0; i < carTechStateCount; i++)
            {
                carTechStates.Add(new CarTechState
                {
                    CarId = rand.Next(firstCarId, carCount + 1),
                    InspectorId = rand.Next(firstInspectorId, inspectorCount + 1),
                    Date = new DateTime(rand.Next(2000, DateTime.Now.Year + 1), rand.Next(1, 13), rand.Next(1, 29)),
                    Mileage = rand.NextDouble() * 1000.0,
                    BrakeSystem = "Тормозная_система_" + rand.Next(1, 6),
                    Suspension = "Подвеска_" + rand.Next(1, 6),
                    Wheels = "Колёса_" + rand.Next(1, 6),
                    Lightning = "Осветительный_прибор_" + rand.Next(1, 6),
                    MarkOnPassageOfServiceStation = rand.NextBool()
                });
            }

            context.CarTechStates.AddRange(carTechStates);
            context.SaveChanges();
        }
    }
}
