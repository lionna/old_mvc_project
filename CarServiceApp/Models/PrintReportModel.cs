namespace CarServiceApp.Models
{
    public class PrintReportModel<T>
    {
        public DateTime? SelectedDate { get; set; }
        public IEnumerable<T> List { get; set; }
        public List<decimal> TotalMoneys { get; set; }
        public string SeletedMonthName { get; set; }
        public int? SelectedYear { get; set; }
        public Dictionary<int, string> Months { get; set; }
        public string FullName { get; set; }
    }
}
