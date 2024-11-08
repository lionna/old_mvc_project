namespace CarServiceApp.Models
{
    public class SearchPreRecordsViewModel
    {
        private DateTime? dateMakingFrom;
        private DateTime? dateMakingBefore;
        public long? RecordId { set; get; }
        public string NameClient { set; get; }
        public string MarkCar { set; get; }
        public string RegNumber { set; get; }
        public DateTime? DateMakingFrom { set { dateMakingFrom = value ?? new DateTime(); } get { return dateMakingFrom; } }
        public DateTime? DateMakingBefore { set { dateMakingBefore = value ?? new DateTime(2200, 1, 1); } get { return dateMakingBefore; } }
        public bool? IsRejection { set; get; }
    }
}