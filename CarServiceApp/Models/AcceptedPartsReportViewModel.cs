using CarServiceApp.Domain.Entity.Models;

namespace CarServiceApp.Models
{
    public class AcceptedPartsReportViewModel
    {
        public string NameClient { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string NameEmployee { get; set; }
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public List<AcceptanceCustomSpart> SpareParts { get; set; }
    }
}