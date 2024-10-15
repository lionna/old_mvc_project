using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service;

namespace CarServiceApp.Models
{
    public class AcceptancedPartsViewModel
    {
        public List<AcceptancedSparePart> AcceptencedParts { set; get; }
        public InvoiceInformation InvoiceInfo { set; get; }
        public decimal? TotallSummByParts { set; get; }
    }
}