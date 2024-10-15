using CarServiceApp.Domain.Entity.Models;

namespace CarServiceApp.Models
{
    public class ContractsViewModel
    {
        public string FullName { set; get; }
        public int PersonnelNumber { set; get; }
        public IEnumerable<ContractToEmployee> Contracts { set; get; }
    }
}
