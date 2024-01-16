namespace Wallet.Contract.Abstraction
{
    public interface ISortableQueryField
    {
        string? SortField { get; set; }

        string? SortDirection { get; set; }
    }
}