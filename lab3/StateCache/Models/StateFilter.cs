using System.ComponentModel;

namespace StateCache.Models
{
    public class StateFilter
    {
        [DisplayName("Тормозная система")]
        public string BrakeSystem { get; set; }
        [DisplayName("Подвеска")]
        public string Suspension { get; set; }
    }
}
