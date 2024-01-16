using Wallet.Domain.Repositories;

namespace Wallet.Api.Integration.Test.Tests.Wallet
{
    public class WalletFixture
    {
        public WalletFixture(IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository, IWalletRepositoryForTest walletRepositoryForTest)
        {
            this.walletRepository = walletRepository;
            this.walletTransactionRepository = walletTransactionRepository;
            this.walletRepositoryForTest = walletRepositoryForTest;
        }

        public IWalletRepository walletRepository { get; }
        public IWalletRepositoryForTest walletRepositoryForTest { get; }

        public IWalletTransactionRepository walletTransactionRepository { get; }
    }
}
