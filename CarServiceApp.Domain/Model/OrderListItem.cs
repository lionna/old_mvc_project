namespace CarServiceApp.Domain.Model
{
    public class OrderListItem
    {
        public string OrderId { get; set; }
        public DateTime? DateMakingOrder { get; set; }
        public DateTime? DateFactCompleting { get; set; }
        public string FullName { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
    }
}