namespace CarServiceApp.Domain.Model
{
    public partial class ReportByOrderModel
    {
        public string OrderId { get; set; }
        public DateTime? DateMaking { get; set; }
        public string FullName { get; set; }
        public string Model { get; set; }
        public string RegisterNumber { get; set; }
        public int? IssueYear { get; set; }
        public decimal? CostService { get; set; }
        public decimal? TaxAddedValue { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotallPriceParts { get; set; }
        public decimal? TotallPriceOrder { get; set; }
    }
}