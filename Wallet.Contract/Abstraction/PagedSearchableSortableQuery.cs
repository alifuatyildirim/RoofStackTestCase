namespace Wallet.Contract.Abstraction
{
    public abstract class PagedSearchableSortableQuery : PagedSortableQuery, ISearchableQuery
    {
        public string? SearchText { get; set; }
    }
}