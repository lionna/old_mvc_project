using CarServiceApp.Domain.Entity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class ServiceViewModel
    {
        public string OrderId { get; set; }
        public virtual Service Service { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual SelectList ListEmployees { get; set; }
        public int? PersonnelNumber { set; get; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "Требуеться ввести время")]
        [Display(Name = "Затраченное время")]
        public double? TakeTime { get; set; }
        [Display(Name = "Дата")]
        public DateTime? DateCompleting { get; set; }
        public string ServiceId { get; set; }
        public int TaxAddedValue { get; set; }
        public bool IsEnableChange { set; get; }
    }
}
