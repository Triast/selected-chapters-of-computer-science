﻿using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Models
{
    public class CarTechState
    {
        public int CarTechStateId { get; set; }
        public int CarId { get; set; }
        public int InspectorId { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        // Пробег
        public double Mileage { get; set; }
        // Тормозная система
        [Required]
        public string BrakeSystem { get; set; }
        // Подвеска
        [Required]
        public string Suspension { get; set; }
        [Required]
        public string Wheels { get; set; }
        // Осветительные приборы
        [Required]
        public string Lightning { get; set; }
        // Доп. оборудование
        public string AdditionalEquipment { get; set; }
        // Отметка о прохождении СТО
        public bool MarkOnPassageOfServiceStation { get; set; }

        public virtual Car Car { get; set; }
        public virtual Inspector Inspector { get; set; }

        public override string ToString()
        {
            return "( ИД = " + CarTechStateId + ", ИД авто = " + CarId +
                ", ИД инспектора = " + InspectorId + ", дата = " +
                Date.ToShortDateString() + ", пробег = " + Mileage +
                ", тормозная система = " + BrakeSystem + ", подвеска" +
                Suspension + ", колёса = " + Wheels + ", осветительные приборы = " +
                Lightning + ", доп. оборудование = " + (AdditionalEquipment ?? "нет") +
                ", отметка о прохождении СТО = " + MarkOnPassageOfServiceStation +
                " )";
        }
    }
}
