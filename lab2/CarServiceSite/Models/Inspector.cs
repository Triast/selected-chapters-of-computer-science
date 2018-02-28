using System.Collections.Generic;

namespace CarServiceSite.Models
{
    public class Inspector
    {
        public int InspectorId { get; set; }
        public int FullName { get; set; }
        public string Subdivision { get; set; }

        public virtual List<CarTechState> CarTechStates { get; set; }
    }
}
