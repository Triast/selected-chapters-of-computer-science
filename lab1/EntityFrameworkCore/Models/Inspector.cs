using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Models
{
    public class Inspector
    {
        public int InspectorId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Subdivision { get; set; }

        public virtual List<CarTechState> CarTechStates { get; set; }

        public override string ToString()
        {
            return $"( ИД = {InspectorId}, Имя = {FullName}, Подразделение = {Subdivision} )";
        }
    }
}
