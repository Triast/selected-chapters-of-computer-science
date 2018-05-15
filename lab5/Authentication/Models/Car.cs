using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [DisplayName("Госномер автомобиля")]
        [Required]
        public string StateNumber { get; set; }
        [DisplayName("Номер техпаспорта")]
        [Required]
        public string TechnicalPassport { get; set; }
        [DisplayName("Марка")]
        [Required]
        public string Mark { get; set; }
        [DisplayName("Объём двигателя")]
        public int EngineVolume { get; set; }
        [DisplayName("Номер кузова")]
        [Required]
        public string BodyNumber { get; set; }
        [DisplayName("Номер двигателя")]
        [Required]
        public string EngineNumber { get; set; }
        [DisplayName("Год выпуска")]
        public int ReleaseYear { get; set; }
        [DisplayName("ФИО владельца")]
        [Required]
        public string OwnerName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual List<CarTechState> CarTechState { get; set; }
    }
}
