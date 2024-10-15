using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service;

namespace CarServiceApp.Models
{
    public class AcceptedCustomPartsViewModel
    {
        public List<AcceptanceCustomSpartModel> AcceptencedParts { set; get; }
        public CustomInvoiceInfo CustomInvoiceInfo { set; get; }
    }
}
