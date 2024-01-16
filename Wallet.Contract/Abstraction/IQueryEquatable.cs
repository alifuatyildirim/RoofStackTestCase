namespace Wallet.Contract.Abstraction
{
    public interface IQueryEquatable<in T>
    {
        public bool RequestEquals(T query);
    }
}