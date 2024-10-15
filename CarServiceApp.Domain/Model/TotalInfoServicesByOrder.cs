namespace CarServiceApp.Domain.Model
{
    public class TotalInfoServicesByOrder
    {
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public decimal? PriceHour { get; set; }
        public double? RateTime { get; set; }
        public double? TakeTime { get; set; }
        public int? TaxAddedValue { get; set; }
        public double? SummMoney { get; set; }
        public DateTime? DateCompleting { get; set; }
        public string FullName { get; set; }
    }
}