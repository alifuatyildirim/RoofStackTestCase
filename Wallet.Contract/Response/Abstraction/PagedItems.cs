namespace Wallet.Contract.Response.Abstraction
{
    public abstract class PagedItems<TModel> 
        where TModel : class
    {
        public int TotalCount { get; set; }

        public IReadOnlyCollection<TModel>? Rows { get; set; }
    }
}