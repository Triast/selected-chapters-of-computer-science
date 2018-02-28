using System.Collections.Generic;

namespace CarServiceSite.Models
{
    public class Car
    {
        public int CarId { get; set; }
        // Госномер автомобиля
        public string StateNumber { get; set; }
        // Номер техпаспорта
        public string TechnicalPassport { get; set; }
        // Марка
        public string Mark { get; set; }
        // Объём двигателя
        public double EngineVolume { get; set; }
        // Номер кузова
        public string BodyNumber { get; set; }
        // Номер двигателя
        public string EngineNumber { get; set; }
        // Год выпуска
        public int ReleaseYear { get; set; }
        // ФИО владельца
        public string OwnerName { get; set; }

        public virtual List<CarTechState> CarTechState { get; set; }
    }
}
