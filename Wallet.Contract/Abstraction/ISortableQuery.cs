namespace Wallet.Contract.Abstraction
{
    public interface ISortableQuery : ISortableQueryField
    {
        string DefaultSortableField { get; }
        
        bool DefaultDirectionIsAscending { get; }

        IReadOnlyCollection<string> SortableFields { get; }
    }
}