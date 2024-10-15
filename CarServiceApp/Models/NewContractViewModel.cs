using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class NewContractViewModel
    {
        [Display(Name = "ФИО")]
        public string FullName { set; get; }
        [Display(Name = "Номер")]
        public int? PersonnelNumber { set; get; }
        [Display(Name = "Дата найма")]
        public DateOnly? RecruitDate { set; get; }
        [Display(Name = "Тип")]
        public string Type { set; get; }
        [Display(Name = "Срок")]
        public int? Term { set; get; }
    }
}
