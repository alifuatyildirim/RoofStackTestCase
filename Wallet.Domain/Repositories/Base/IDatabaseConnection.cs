namespace Wallet.Domain.Repositories;

public interface IDatabaseConnection
{
    Task<ITransactionScope> BeginTransactionScopeAsync();

    ITransactionScope BeginTransactionScope();
}