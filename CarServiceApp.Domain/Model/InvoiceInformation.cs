namespace CarServiceApp.Domain.Model
{
    public partial class InvoiceInformation
    {
        public string ProviderName { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public string ProviderId { get; set; }
        public string FullName { get; set; }
        public int PersonnelNumber { get; set; }
        public int LotNumber { get; set; }
    }
}