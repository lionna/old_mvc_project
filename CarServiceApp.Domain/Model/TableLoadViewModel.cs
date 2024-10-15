using CarServiceApp.Domain.Entity.Models;

namespace CarServiceApp.Domain.Model
{
    public class TableLoadViewModel
    {
        public DateTime? CurrentDate { set; get; }
        public OrderService Order { set; get; }
        public string NameCar { set; get; }
        public List<PreRecordService> PreRecordServices { set; get; }
        public List<LoadTableModel> LoadEmployees { set; get; }
    }

    public class TableLoadPreRecordViewModel
    {
        public PreRecord Record { set; get; }
        public List<PreRecordService> PreRecordServices { set; get; }
        public List<LoadTableModel> LoadEmployees { set; get; }
    }
}