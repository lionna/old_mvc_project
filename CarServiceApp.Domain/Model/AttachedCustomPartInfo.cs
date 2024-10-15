namespace CarServiceApp.Domain.Model
{
    public class AttachedCustomPartInfo
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Manufacture { get; set; }
        public string Unit { get; set; }
        public double? Number { get; set; }
        public string StateSPart { get; set; }
        public int AcceptanceId { get; set; }
    }
}