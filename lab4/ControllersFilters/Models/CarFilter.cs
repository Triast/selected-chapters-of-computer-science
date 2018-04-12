using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ControllersFilters.Models
{
    public class CarFilter
    {
        [DisplayName("Госномер автомобиля")]
        public string StateNumber { get; set; }
        [DisplayName("Марка")]
        public string Mark { get; set; }
        [DisplayName("Год выпуска")]
        public int ReleaseYear { get; set; }
    }
}
