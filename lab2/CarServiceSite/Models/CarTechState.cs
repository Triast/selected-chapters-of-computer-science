namespace CarServiceSite.Models
{
    public class CarTechState
    {
        public int CarTechStateId { get; set; }
        public int CarId { get; set; }
        public int InspectorId { get; set; }
        public System.DateTime Date { get; set; }
        // Пробег
        public double Mileage { get; set; }
        // Тормозная система
        public string BrakeSystem { get; set; }
        // Подвеска
        public string Suspension { get; set; }
        public string Wheels { get; set; }
        // Осветительные приборы
        public string Lightning { get; set; }
        // Доп. оборудование
        public string AdditionalEquipment { get; set; }
        // отметка о прохождении СТО
        public bool MarkOnPassageOfServiceStation { get; set; }

        public virtual Car Car { get; set; }
        public virtual Inspector Inspector { get; set; }
    }
}
