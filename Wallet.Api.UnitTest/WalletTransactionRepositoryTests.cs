using System.Linq.Expressions; 
using Moq; 
using Wallet.Common.Enums;
using Wallet.Data.Repositories;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

[TestFixture]
public class WalletTransactionRepositoryTests
{
    [Test]
    public async Task CreateAsync_Should_Return_New_Transaction_Id()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<WalletTransaction, Guid>>();
        var walletTransactionRepository = new WalletTransactionRepository(genericRepositoryMock.Object);

        Guid transactionId = Guid.NewGuid();
        Guid walletId = Guid.NewGuid();
        var walletTransaction = new WalletTransaction
        {
            Id= transactionId,
            WalletId = walletId,
            TransactionType = TransactionType.Deposit,
            Amount = 100
        };

        genericRepositoryMock.Setup(repo => repo.AddOneAsync(walletTransaction, It.IsAny<ITransactionScope>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await walletTransactionRepository.CreateAsync(walletTransaction, null);

        // Assert
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public async Task GetAllTransactions_Should_Return_Transaction_List_And_Count()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<WalletTransaction, Guid>>();
        var walletTransactionRepository = new WalletTransactionRepository(genericRepositoryMock.Object);
 
        Guid walletId = Guid.NewGuid();
        var transactions = new List<WalletTransaction>
        {
            new WalletTransaction
            {
                Id =  Guid.NewGuid(),
                WalletId = walletId,
                TransactionType = TransactionType.Deposit,
                Amount = 50 
            },
            new WalletTransaction
            {
                Id =  Guid.NewGuid(),
                WalletId = walletId,
                TransactionType = TransactionType.Deposit,
                Amount = 150 
            },
            new WalletTransaction
            {
                Id =  Guid.NewGuid(),
                WalletId = walletId,
                TransactionType = TransactionType.WithDraw,
                Amount = 10 
            },
        };

        genericRepositoryMock.Setup(repo => repo.Query().Where(It.IsAny<Expression<Func<WalletTransaction, bool>>>()).ToListAsync())
            .ReturnsAsync(transactions);

        // Act
        var result = await walletTransactionRepository.GetAllTransactions(walletId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(transactions.Count, result.Count);
    }

    [Test]
    public async Task BeginTransactionScopeAsync_Should_Return_Transaction_Scope()
    {
        // Arrange
        var genericRepositoryMock = new Mock<IGenericRepository<WalletTransaction, Guid>>();
        var walletTransactionRepository = new WalletTransactionRepository(genericRepositoryMock.Object);

        var mockTransactionScope = new Mock<ITransactionScope>();
        genericRepositoryMock.Setup(repo => repo.BeginTransactionScopeAsync()).ReturnsAsync(mockTransactionScope.Object);

        // Act
        var result = await walletTransactionRepository.BeginTransactionScopeAsync();

        // Assert
        Assert.IsNotNull(result);
        mockTransactionScope.Verify(ts => ts.BeginTransaction(), Times.Once);
    }
}
