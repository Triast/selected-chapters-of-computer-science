using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControllersFilters.Models
{
    public class InspectorsViewModel : ICarSiteViewModel<Inspector>
    {
        static Inspector inspector;

        public Inspector Entity => inspector;

        public IEnumerable<Inspector> Collection { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
    }
}
