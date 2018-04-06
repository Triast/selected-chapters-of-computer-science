using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StateCache.Models
{
    public class CarTechStatesViewModel : ICarSiteViewModel<CarTechState>
    {
        static CarTechState state = new CarTechState();
        
        public CarTechState Entity => state;

        public IEnumerable<CarTechState> Collection { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
    }
}
