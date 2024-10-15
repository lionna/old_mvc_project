using CarServiceApp.Domain.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Models
{
    public class ReportModel
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? SortRow { get; set; }
        public bool? IsAsc { get; set; }
        public SelectList Months { get; set; }
        public SelectList Years { get; set; }
        public GridItem GridItem { get; set; }
        public int? Page { get; set; }
    }
}
