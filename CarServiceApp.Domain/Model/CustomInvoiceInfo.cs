namespace CarServiceApp.Domain.Model
{
    public class CustomInvoiceInfo
    {
        public string ClientName { get; set; }
        public DateTime? Date { get; set; }
        public string ClientId { get; set; }
        public string EmployeeName { get; set; }
        public int PersonnelNumber { get; set; }
        public int AcceptanceId { get; set; }
    }
}