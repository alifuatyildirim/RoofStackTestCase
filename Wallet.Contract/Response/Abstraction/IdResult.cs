namespace Wallet.Contract.Response.Abstraction
{
    public record IdResult<T>
        where T : IEquatable<T>
    {
        public T Id { get; protected set; } = default!;
    }
}