using Wallet.Common.Constants;

namespace Wallet.Contract.Abstraction
{
    public abstract class PagedQuery
    {
        private int limit = PagingConstants.PageLimit;
        private int offset;
 
        public int Limit
        {
            get => this.limit;
            set => this.limit = (value <= 0 || value > 1000) && (value != PagingConstants.ExcelLimit) ? PagingConstants.PageLimit : value;
        }
 
        public int Offset
        {
            get => this.offset;
            set => this.offset = value <= 0 ? 0 : value;
        }
    }
}