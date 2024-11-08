namespace CarServiceApp.Models
{
    public class SearchPartsOfStoreViewModel
    {
        private DateTime? dateIvoiceBefore;
        private decimal priceBefore;
        private float stockPercent;
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Manufacture { get; set; }
        public decimal? PriceBefore { set { priceBefore = value ?? 100000000; } get { return priceBefore; } }
        public decimal? PriceAfter { get; set; }
        public int? InvoiceId { get; set; }
        public int? PersonnelNumber { get; set; }
        public float? StockPercent { set { stockPercent = value ?? 100; } get { return stockPercent; } }
        public DateTime? DateIvoiceFrom { get; set; }
        public DateTime? DateIvoiceBefore { set { dateIvoiceBefore = value ?? new DateTime(2020, 1, 1); } get { return this.dateIvoiceBefore; } }
    }
}