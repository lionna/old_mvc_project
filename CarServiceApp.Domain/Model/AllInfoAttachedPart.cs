namespace CarServiceApp.Domain.Model
{
    public class AllInfoAttachedPart
    {
        public string Name { get; set; }
        public string PartId { get; set; }
        public string Manufacture { get; set; }
        public int? LotNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? CostPart { get; set; }
        public string Unit { get; set; }
        public double? Number { get; set; }
        public double? Summ { get; set; }
        public string FullName { get; set; }
    }
}