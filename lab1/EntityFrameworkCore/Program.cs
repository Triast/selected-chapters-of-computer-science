using System;
using EntityFrameworkCore.Models;
using System.Linq;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CarServiceContext())
            {
                CarServiceInitializer.Initialize(context);

                PrintQuery(
                    "Выборка всех данных из таблицы, стоящей в схеме базы" +
                    " данных нас стороне отношения «один»:",
                    context.Inspectors.ToList()
                );

                PrintQuery(
                    "Выборка данных из таблицы, стоящей в схеме базы данных" +
                    " нас стороне отношения «один», отфильтрованные по" +
                    " определенному условию, налагающему ограничения на одно" +
                    " или несколько полей:",
                    context.Inspectors.Where(i => i.Subdivision.Contains("1"))
                );

                PrintQuery(
                    "Выборка данных, сгруппированных по любому из полей данных" +
                    " с выводом какого-либо итогового результата (min," +
                    " max, avg, сount или др.) по выбранному полю из таблицы," +
                    " стоящей в схеме базы данных нас стороне отношения «многие»:",
                    context
                        .CarTechStates
                        .GroupBy(c => c.MarkOnPassageOfServiceStation, c => c.CarTechStateId)
                        .Select(group => new 
                        {
                            MarkOnPassage = group.Key,
                            PassedCarService = group.Count()
                        })
                );

                PrintQuery(
                    "Выборка данных из двух полей двух таблиц, связанных" +
                    " между собой отношением «один-ко-многим»:",
                    context.CarTechStates.Include("Inspector").Select(c => new 
                    {
                        c.CarTechStateId,
                        InspectorName = c.Inspector.FullName
                    })
                );

                PrintQuery(
                    "Выборка данных из двух таблиц, связанных между собой" +
                    " отношением «один-ко-многим» и отфильтрованным по" +
                    " некоторому условию, налагающему ограничения на значения" +
                    " одного или нескольких полей:",
                    context
                        .CarTechStates
                        .Include("Inspector")
                        .Where(c => c.Date < DateTime.Now.AddYears(-10))
                        .Select(c => new {
                            c.CarTechStateId,
                            c.Date,
                            InspectorName = c.Inspector.FullName
                        })
                );

                InsertInspector("Вставка данных в таблицу, стоящей на стороне" +
                    " отношения «Один»:", context
                );

                InsertCarTechState("Вставка данных в таблицу, стоящей на" +
                    " стороне отношения «Многие»", context
                );

                Remove(
                    "Удаление данных из таблиц, стоящих на стороне отношения" +
                    " «Один» и «Многие»", context
                );

                UpdateMileage(
                    "Обновление удовлетворяющих определенному условию записей" +
                    " в любой из таблиц базы данных", context
                );
            }
        }

        static void PrintQuery(string comment, IEnumerable query)
        {
            Console.WriteLine(comment);

            foreach (var item in query)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("Для продолжения нажмите клавишу Enter...");
            Console.ReadLine();
        }

        static void Insert<T>(string comment, string type, T obj, CarServiceContext context)
            where T: class
        {
            Console.WriteLine(comment);
            Console.WriteLine(type);
            Console.WriteLine(obj);

            Console.WriteLine("Кол-во записей до добавления: " + context.Set<T>().Count());

            context.Set<T>().Add(obj);
            context.SaveChanges();

            Console.WriteLine("Кол-во записей после добавления: " + context.Set<T>().Count());

            Console.WriteLine();
            Console.WriteLine("Для продолжения нажмите клавишу Enter...");
            Console.ReadLine();
        }

        static void InsertInspector(string comment, CarServiceContext context)
        {
            var rand = new Random();

            var inspector = new Inspector
            {
                FullName = new FullName().ToString(),
                Subdivision = "Подразделение_" + rand.Next(1, 6)
            };

            Insert(comment, "Инспектор", inspector, context);
        }

        static void InsertCarTechState(string comment, CarServiceContext context)
        {
            var rand = new Random();

            int firstInspectorId = context.Inspectors.First().InspectorId;
            int firstCarId = context.Cars.First().CarId;

            int lastInspectorId = context.Inspectors.Last().InspectorId;
            int lastCarId = context.Cars.Last().CarId;

            var state = new CarTechState
            {
                CarId = rand.Next(firstCarId, lastCarId + 1),
                InspectorId = rand.Next(firstInspectorId, lastInspectorId + 1),
                Date = new DateTime(rand.Next(2000, DateTime.Now.Year + 1), rand.Next(1, 13), rand.Next(1, 29)),
                Mileage = rand.NextDouble() * 1000.0,
                BrakeSystem = "Тормозная_система_" + rand.Next(1, 6),
                Suspension = "Подвеска_" + rand.Next(1, 6),
                Wheels = "Колёса_" + rand.Next(1, 6),
                Lightning = "Осветительный_прибор_" + rand.Next(1, 6),
                MarkOnPassageOfServiceStation = rand.NextBool()
            };

            Insert(comment, "Car tech state", state, context);
        }

        static void Remove(string comment, CarServiceContext context)
        {
            Console.WriteLine(comment);

            var inspectorCountBefore = context.Inspectors.Count();
            var statesCountBefore = context.CarTechStates.Count();

            var inspectors = context.Inspectors.Where(i => i.Subdivision.Contains("5"));
            var carTechStates = context
                                    .CarTechStates
                                    .Include("Inspector")
                                    .Where(c => c.Inspector.Subdivision.Contains("5"));

            context.CarTechStates.RemoveRange(carTechStates);
            context.SaveChanges();

            context.Inspectors.RemoveRange(inspectors);
            context.SaveChanges();

            var inspectorCountAfter = context.Inspectors.Count();
            var statesCountAfter = context.CarTechStates.Count();

            Console.WriteLine("Кол-во инспекторов до удаления - " + inspectorCountBefore);
            Console.WriteLine("Кол-во инспекторов после удаления - " + inspectorCountAfter);

            Console.WriteLine("\nКол-во записей о тех. состоянии автомобилей до удаления - " + statesCountBefore);
            Console.WriteLine("Кол-во записей о тех. состоянии автомобилей после удаления - " + statesCountAfter);

            Console.WriteLine();
            Console.WriteLine("Для продолжения нажмите клавишу Enter...");
            Console.ReadLine();
        }

        static void UpdateMileage(string comment, CarServiceContext context)
        {
            Console.WriteLine(comment);

            var states = context.CarTechStates.Where(c => c.Date > DateTime.Now.AddYears(-10));

            PrintQuery("До обновления:", states.Take(10));

            foreach (var item in states)
            {
                item.Mileage += 1000.0;
            }

            context.SaveChanges();

            PrintQuery("После обновления:", states.Take(10));

            Console.WriteLine();
            Console.WriteLine("Для продолжения нажмите клавишу Enter...");
            Console.ReadLine();
        }
    }
}
