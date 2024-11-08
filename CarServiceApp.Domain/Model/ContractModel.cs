namespace CarServiceApp.Domain.Model
{
    public class ContractModel
    {
        public int ContractId { get; set; }
        public DateOnly RecruitDate { get; set; }
        public DateOnly? DismissDate { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public int? Term { get; set; }
        public int PersonnelNumber { get; set; }
    }
}