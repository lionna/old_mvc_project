using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service;

namespace CarServiceApp.Models
{
    public class ReportOrderForClientAcceptanceCarViewModel
    {
        public Car Car { get; set; }
        public Client Client { get; set; }
        public List<ReportAcceptedCustomParts> CustomParts { set; get; }
        public string ManagerName { set; get; }
        public Domain.Entity.Models.OrderService OrderService { get; set; }
        public List<RemarkToStateCar> Remarks { get; set; }
        public int N { get; set; }
    }
}