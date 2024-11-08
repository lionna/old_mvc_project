namespace CarServiceApp.Domain.Model
{
    public class SparePartModel
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        public int LotNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int Stock { get; set; }
        public int TradeIncrease { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int PersonnelNumber { get; set; }
    }
}