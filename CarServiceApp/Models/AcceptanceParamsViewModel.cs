using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Models
{
    public class AcceptanceParamsViewModel
    {
        public SelectList ListProvider { set; get; }
        public SelectList ListEmployee { set; get; }
        public int PersonnelNumber { set; get; }
        public string ProviderId { set; get; }
        public int? LotNumber { set; get; }
        public DateTime? DateLotStart { set; get; }
        public DateTime? DateLotEnd { set; get; }
        public DateTime? DeliveryDate { set; get; }
        public string PartId { set; get; }
        public string NamePart { set; get; }
        public SelectList ListProvidersSearch { get; set; }
        public string ProvideId { get; set; }
    }
}
