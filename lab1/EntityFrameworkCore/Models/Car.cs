using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Models
{
    public class Car
    {
        public int CarId { get; set; }
        // Госномер автомобиля
        [Required]
        public string StateNumber { get; set; }
        // Номер техпаспорта
        [Required]
        public string TechnicalPassport { get; set; }
        // Марка
        [Required]
        public string Mark { get; set; }
        // Объём двигателя
        public int EngineVolume { get; set; }
        // Номер кузова
        [Required]
        public string BodyNumber { get; set; }
        // Номер двигателя
        [Required]
        public string EngineNumber { get; set; }
        // Год выпуска
        public int ReleaseYear { get; set; }
        // ФИО владельца
        [Required]
        public string OwnerName { get; set; }

        public virtual List<CarTechState> CarTechState { get; set; }
    }
}
