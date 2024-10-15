namespace CarServiceApp.Models
{
    public class RemarkToStateCarViewModel
    {
        public int RemarkId { get; set; }
        public string OrderId { get; set; }
        public int X_Axis_pos { get; set; }
        public int Y_Axis_pos { get; set; }
        public string RemarkText { get; set; }
        public byte? NumberType { get; set; }

    }
}
