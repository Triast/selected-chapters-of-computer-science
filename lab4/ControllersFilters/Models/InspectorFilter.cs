using System.ComponentModel;

namespace ControllersFilters.Models
{
    public class InspectorFilter
    {
        [DisplayName("ФИО инспектора")]
        public string FullName { get; set; }
        [DisplayName("Подразделение")]
        public string Subdivision { get; set; }
    }
}
