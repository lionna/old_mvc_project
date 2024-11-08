namespace CarServiceApp.Domain.Model
{
    public class ReportByExecServiceByMonthForEmployee
    {
        public string FullName { get; set; }
        public int PersonnelNumber { get; set; }
        public string NamePost { get; set; }
        public int? CountOfOrders { get; set; }
        public int? CountOfServices { get; set; }
        public double? TakeTime { get; set; }
        public decimal? AveragePriceServ { get; set; }
        public decimal? AveragePriceOrder { get; set; }
        public decimal? TotalMoney { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}