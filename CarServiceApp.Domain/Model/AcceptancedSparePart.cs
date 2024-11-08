namespace CarServiceApp.Domain.Model
{
    public partial class AcceptancedSparePart
    {
        public int LotNumber { get; set; }
        public string PartId { get; set; }
        public double? Number { get; set; }
        public decimal? Price { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        public long PositionId { get; set; }
        public double Total { get; set; }
    }
}