namespace Wallet.Contract.Abstraction
{
    public abstract record PagedSearchableSortableQuery : PagedSortableQuery, ISearchableQuery
    {
        public string? SearchText { get; set; }
    }
}