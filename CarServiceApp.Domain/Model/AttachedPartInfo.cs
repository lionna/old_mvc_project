namespace CarServiceApp.Domain.Model
{
    public class AttachedPartInfo
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Manufacture { get; set; }
        public string Unit { get; set; }
        public double? Number { get; set; }
        public decimal? CostPart { get; set; }
        public int LotNumber { get; set; }
        public decimal? Price { get; set; }
        public long PositionId { get; set; }
    }
}