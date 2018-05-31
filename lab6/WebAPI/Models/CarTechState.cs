using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class CarTechState
    {
        public int CarTechStateId { get; set; }
        public int CarId { get; set; }
        public int InspectorId { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Дата")]
        public System.DateTime Date { get; set; }
        [DisplayName("Пробег")]
        public double Mileage { get; set; }
        [DisplayName("Тормозная система")]
        [Required]
        public string BrakeSystem { get; set; }
        [DisplayName("Подвеска")]
        [Required]
        public string Suspension { get; set; }
        [DisplayName("Колёса")]
        [Required]
        public string Wheels { get; set; }
        [DisplayName("Осветительные приборы")]
        [Required]
        public string Lightning { get; set; }
        [DisplayName("Доп. оборудование")]
        public string AdditionalEquipment { get; set; }
        [DisplayName("Прохождение СТО")]
        public bool MarkOnPassageOfServiceStation { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual Car Car { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Inspector Inspector { get; set; }

        //public override string ToString()
        //{
        //    return "( ИД = " + CarTechStateId + ", ИД авто = " + CarId +
        //        ", ИД инспектора = " + InspectorId + ", дата = " +
        //        Date.ToShortDateString() + ", пробег = " + Mileage +
        //        ", тормозная система = " + BrakeSystem + ", подвеска" +
        //        Suspension + ", колёса = " + Wheels + ", осветительные приборы = " +
        //        Lightning + ", доп. оборудование = " + (AdditionalEquipment ?? "нет") +
        //        ", отметка о прохождении СТО = " + MarkOnPassageOfServiceStation +
        //        " )";
        //}
    }
}
