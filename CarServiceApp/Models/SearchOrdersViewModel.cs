namespace CarServiceApp.Models
{
    public class SearchOrdersViewModel
    {
        public string ClientId { set; get; }
        public string OrderId { set; get; }
        public string Model { set; get; }
        public string FullName { set; get; }
        public string RegNumber { set; get; }
        public DateTime? DateMakingFrom { set; get; }
        public DateTime? DateMakingBefore { set; get; }
        public bool IsRejection { set; get; }
        public bool IsClosed { set; get; }
        public byte NumbSortRow { get; set; }
        public bool IsAsc { get; set; }

    }
}