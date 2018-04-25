using System.Collections.Generic;
using System.ComponentModel;

namespace ControllersFilters.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [DisplayName("Госномер автомобиля")]
        public string StateNumber { get; set; }
        [DisplayName("Номер техпаспорта")]
        public string TechnicalPassport { get; set; }
        [DisplayName("Марка")]
        public string Mark { get; set; }
        [DisplayName("Объём двигателя")]
        public int EngineVolume { get; set; }
        [DisplayName("Номер кузова")]
        public string BodyNumber { get; set; }
        [DisplayName("Номер двигателя")]
        public string EngineNumber { get; set; }
        [DisplayName("Год выпуска")]
        public int ReleaseYear { get; set; }
        [DisplayName("ФИО владельца")]
        public string OwnerName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual List<CarTechState> CarTechState { get; set; }
    }
}
