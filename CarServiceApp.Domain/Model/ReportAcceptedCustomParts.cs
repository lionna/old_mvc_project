namespace CarServiceApp.Domain.Model
{
    public class ReportAcceptedCustomParts
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        public string FullName { get; set; }
        public string StateSPart { get; set; }
        public string ClientId { get; set; }
        public DateTime? AcceptDate { get; set; }
        public double? Number { get; set; }
        public double? StockCustom { get; set; }
        public int AcceptanceId { get; set; }
    }
}