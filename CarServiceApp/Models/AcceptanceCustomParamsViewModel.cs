using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Models
{
    public class AcceptanceCustomParamsViewModel
    {
        public SelectList ListClients { set; get; }
        public SelectList ListEmployee { set; get; }
        public int PersonnelNumber { set; get; }
        public string ClientId { set; get; }
        public int? AcceptanceId { set; get; }
        public DateTime? AcceptanceDate { set; get; }
        public string PartId { set; get; }
        public string PartName { set; get; }
    }
}
