using Newtonsoft.Json;

namespace Wallet.Contract.Abstraction
{
    public abstract class PagedSortableQuery : PagedQuery, ISortableQuery
    {
        public string? SortField { get; set; }

        public string? SortDirection { get; set; }
        
        [JsonIgnore]
        public abstract string DefaultSortableField { get; }

        [JsonIgnore]
        public virtual bool DefaultDirectionIsAscending { get; } = true;

        [JsonIgnore]
        public abstract IReadOnlyCollection<string> SortableFields { get; }
    }
}