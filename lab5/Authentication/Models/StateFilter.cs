using System.ComponentModel;

namespace Authentication.Models
{
    public class StateFilter
    {
        [DisplayName("Госномер автомобиля")]
        public string StateNumber { get; set; }
        [DisplayName("ФИО инспектора")]
        public string FullName { get; set; }
        [DisplayName("Тормозная система")]
        public string BrakeSystem { get; set; }
    }
}
