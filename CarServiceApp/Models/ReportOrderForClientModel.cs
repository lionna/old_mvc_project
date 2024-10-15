using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Models
{
    public class GetAllInfoAttachedPartById_Result
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

    public class ReportOrderForClientModel
    {
        public List<TotalInfoServicesByOrder> ServiceList { set; get; }
        public List<AllInfoAttachedPart> PartList { set; get; }
        public List<UsingCustomSpartMat> PartCustomList { set; get; }
        public OrderService Order { set; get; }
    }
}