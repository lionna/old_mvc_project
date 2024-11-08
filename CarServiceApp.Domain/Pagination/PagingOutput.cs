namespace CarServiceApp.Domain.Common;

public class PagingOutput<T> : PagingInfo
{
    public IEnumerable<T> Data { set; get; }
}