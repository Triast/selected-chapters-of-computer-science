using System.Collections.Generic;

namespace EntityFrameworkCore.Models
{
    public class Inspector
    {
        public int InspectorId { get; set; }
        public string FullName { get; set; }
        public string Subdivision { get; set; }

        public virtual List<CarTechState> CarTechStates { get; set; }

        public override string ToString()
        {
            return $"( ИД = {InspectorId}, Имя = {FullName}, Подразделение = {Subdivision} )";
        }
    }
}
