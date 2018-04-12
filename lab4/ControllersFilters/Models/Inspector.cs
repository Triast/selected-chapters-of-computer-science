using System.Collections.Generic;
using System.ComponentModel;

namespace ControllersFilters.Models
{
    public class Inspector
    {
        public int InspectorId { get; set; }
        [DisplayName("ФИО инспектора")]
        public string FullName { get; set; }
        [DisplayName("Подразделение")]
        public string Subdivision { get; set; }

        public virtual List<CarTechState> CarTechStates { get; set; }
    }
}
