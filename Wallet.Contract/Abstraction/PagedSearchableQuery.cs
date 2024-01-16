namespace Wallet.Contract.Abstraction
{
    public abstract record PagedSearchableQuery : PagedQuery, ISearchableQuery
    {
        public string? SearchText { get; set; }
    }
}
