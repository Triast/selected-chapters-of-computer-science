using System.ComponentModel;

namespace Authentication.Models
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
