using System.Linq.Expressions; 
using Moq; 
using Wallet.Common.Enums;
using Wallet.Data.Repositories; 
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

[TestFixture]
public class WalletRepositoryTests
{
    [Test]
    public async Task CreateAsync_Should_Return_New_Wallet_Id()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<Wallet.Domain.Entities.Wallet, Guid>>();
        var walletRepository = new WalletRepository(genericRepositoryMock.Object);

        var wallet = new Wallet.Domain.Entities.Wallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Limit = 100,
            CurrencyCode = CurrencyCode.TRY 
        };

        genericRepositoryMock.Setup(repo => repo.AddOneAsync(wallet, It.IsAny<ITransactionScope>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await walletRepository.CreateAsync(wallet, null);

        // Assert
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public async Task UpdateAsync_Should_Return_Updated_Wallet_Id()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<Wallet.Domain.Entities.Wallet, Guid>>();
        var walletRepository = new WalletRepository(genericRepositoryMock.Object);

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var wallet = new Wallet.Domain.Entities.Wallet
        {
            Id = walletId,
            UserId = userId,
            Limit = 100,
            CurrencyCode = CurrencyCode.TRY 
        };

        genericRepositoryMock.Setup(repo => repo.UpdateAsync(wallet, It.IsAny<ITransactionScope>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await walletRepository.UpdateAsync(wallet, null);

        // Assert
        Assert.AreEqual(wallet.Id, result);
    }

    [Test]
    public async Task GetByUserIdAsync_Should_Return_Wallet_List()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<Wallet.Domain.Entities.Wallet, Guid>>();
        var walletRepository = new WalletRepository(genericRepositoryMock.Object);

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var wallets = new List<Wallet.Domain.Entities.Wallet>
        {
            new Wallet.Domain.Entities.Wallet
            {
                Id = walletId,
                UserId = userId,
                Limit = 100,
                CurrencyCode = CurrencyCode.TRY 
            },
            new Wallet.Domain.Entities.Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Limit = 300,
                CurrencyCode = CurrencyCode.Dolar 
            },
        };

        genericRepositoryMock.Setup(repo => repo.Query().Where(It.IsAny<Expression<Func<Wallet.Domain.Entities.Wallet, bool>>>()).ToListAsync())
            .ReturnsAsync(wallets);

        // Act
        var result = await walletRepository.GetByUserIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(wallets.Count, result.Count);
    }

    [Test]
    public async Task GetAsync_Should_Return_Wallet()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<Wallet.Domain.Entities.Wallet, Guid>>();
        var walletRepository = new WalletRepository(genericRepositoryMock.Object);

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var wallet = new Wallet.Domain.Entities.Wallet
        {
            Id = walletId,
            UserId = userId,
            Limit = 300,
            CurrencyCode = CurrencyCode.Dolar 
        };

        genericRepositoryMock.Setup(repo => repo.GetByIdAsync(walletId))
            .ReturnsAsync(wallet);

        // Act
        var result = await walletRepository.GetAsync(walletId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(walletId, result.Id);
    }
}
