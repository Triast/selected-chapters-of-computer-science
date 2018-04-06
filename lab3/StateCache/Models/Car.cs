using System.Collections.Generic;
using System.ComponentModel;

namespace StateCache.Models
{
    public class Car
    {
        public int CarId { get; set; }
        // Госномер автомобиля
        [DisplayName("Госномер автомобиля")]
        public string StateNumber { get; set; }
        // Номер техпаспорта
        [DisplayName("Номер техпаспорта")]
        public string TechnicalPassport { get; set; }
        // Марка
        [DisplayName("Марка")]
        public string Mark { get; set; }
        // Объём двигателя
        [DisplayName("Объём двигателя")]
        public int EngineVolume { get; set; }
        // Номер кузова
        [DisplayName("Номер кузова")]
        public string BodyNumber { get; set; }
        // Номер двигателя
        [DisplayName("Номер двигателя")]
        public string EngineNumber { get; set; }
        // Год выпуска
        [DisplayName("Год выпуска")]
        public int ReleaseYear { get; set; }
        // ФИО владельца
        [DisplayName("ФИО владельца")]
        public string OwnerName { get; set; }

        public virtual List<CarTechState> CarTechState { get; set; }
    }
}
