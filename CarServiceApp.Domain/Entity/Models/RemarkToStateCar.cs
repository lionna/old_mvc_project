namespace CarServiceApp.Domain.Entity.Models;

public partial class RemarkToStateCar
{
    public int RemarkId { get; set; }

    public string OrderId { get; set; } = null!;

    public int XAxisPos { get; set; }

    public int YAxisPos { get; set; }

    public string RemarkText { get; set; }

    public byte? NumberType { get; set; }

    public virtual OrderService OrderService { get; set; } = null!;
}
