using CarServiceApp.Domain.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Models
{
    public class ChoosingParamsAnalysis
    {
        public ChoosingParamsAnalysis()
        {
            var collect = new Dictionary<int, string> { 
                { 0, "Выбрать период..." }, 
                { 1, "Месяц" },
                { 2, "Квартал" }, 
                { 3, "День" } };
            this.TypePeriod = new SelectList(collect, "Key", "Value");
        }
        public SelectList TypePeriod { set; get; }
        public byte TypePeriodId { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { get; set; }
        public GridItem GridItem { get; set; }
    }
}