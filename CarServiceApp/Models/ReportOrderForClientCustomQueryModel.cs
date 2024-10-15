using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Models
{
    public class ReportOrderForClientCustomQueryModel
    {
        public List<TotalInfoServicesByOrder> ServiceList { set; get; }
        public OrderService Order { set; get; }
    }
}