using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class Inspector
    {
        public int InspectorId { get; set; }
        [DisplayName("ФИО инспектора")]
        [Required]
        public string FullName { get; set; }
        [DisplayName("Подразделение")]
        [Required]
        public string Subdivision { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual List<CarTechState> CarTechStates { get; set; }
    }
}
