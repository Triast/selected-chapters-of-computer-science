using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateCache.Models
{
    public class CarsViewModel : ICarSiteViewModel<Car>
    {
        static Car sample = new Car();

        public Car Entity => sample;

        public IEnumerable<Car> Collection { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
    }
}
