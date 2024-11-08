using CarServiceApp.Domain.Common;

namespace CarServiceApp.Models
{
    public class SearchParmsContractViewModel
    {
        public int? ContractId { set; get; }
        public string FullName { set; get; }
        public string ContractType { set; get; }
        public int? Term { set; get; }
        public bool IsOn { set; get; }
        public DateOnly? DateFrom { set; get; }
        public DateOnly? DateTo { set; get; }
        public GridItem GridItem { get; set; }
    }
}
