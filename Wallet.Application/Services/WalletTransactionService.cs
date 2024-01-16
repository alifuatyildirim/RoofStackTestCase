using MapsterMapper;
using Wallet.Application.Services.Abstraction; 
using Wallet.Contract; 
using Wallet.Contract.Command.WalletTransaction; 
using Wallet.Contract.Query.WalletTransaction; 
using Wallet.Contract.Response.WalletTransaction;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;

namespace Wallet.Application.Services;

public class WalletTransactionService : IWalletTransactionService
{
    private readonly IWalletService walletService;
    private readonly IWalletTransactionRepository walletTransactionRepository;
    private readonly IMapper mapper;

    public WalletTransactionService(IWalletTransactionRepository walletTransactionRepository,IWalletService walletService, IMapper mapper)
    {
        this.walletTransactionRepository = walletTransactionRepository;
        this.walletService = walletService;
        this.mapper = mapper;
    } 

    public async Task<GuidIdResult> CreateWalletTransactionAsync(CreateWalletTransactionCommand command, CancellationToken cancellationToken)
    {
        using ITransactionScope transactionScope = await this.walletTransactionRepository.BeginTransactionScopeAsync();
        
        WalletTransaction walletTransaction = this.mapper.Map<WalletTransaction>(command);
        var transactionId = await this.walletTransactionRepository.CreateAsync(walletTransaction,transactionScope);

        await this.walletService.UpdateWalletAsync(command, transactionScope);
        
        
        await transactionScope.CommitTransactionAsync();
        
        return new GuidIdResult(transactionId);
    }

    public async Task<WalletTransactionResponse> GetAllWalletTransactionsAsync(GetAllWalletTransactionQuery request, CancellationToken cancellationToken)
    {
        (IReadOnlyCollection<WalletTransaction> walletTransactions, int count) = await this.walletTransactionRepository.GetAllTransactions(request.WalletId);
        return new WalletTransactionResponse()
        {
            Rows = this.mapper.Map<IReadOnlyCollection<WalletTransactionResponseItem>>(walletTransactions),
            TotalCount = count,
        };
        
    }
}