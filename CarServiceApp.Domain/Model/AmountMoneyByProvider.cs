namespace CarServiceApp.Domain.Model
{
    public partial class AmountMoneyByProvider
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string LotNumber { get; set; }
        public double? Amount { get; set; }
    }
}