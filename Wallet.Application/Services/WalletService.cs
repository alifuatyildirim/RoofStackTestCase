using System.Net;
using MapsterMapper;
using Wallet.Application.Services.Abstraction;
using Wallet.Common.Codes;
using Wallet.Common.Enums;
using Wallet.Common.ExceptionHandling;
using Wallet.Common.Extensions;
using Wallet.Contract;
using Wallet.Contract.Command.Wallet;
using Wallet.Contract.Command.WalletTransaction;
using Wallet.Contract.Query.Wallet;
using Wallet.Contract.Response.Wallet; 
using Wallet.Domain.Repositories;

namespace Wallet.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository walletRepository;
    private readonly IMapper mapper;

    public WalletService(IWalletRepository walletRepository,IMapper mapper)
    {
        this.walletRepository = walletRepository;
        this.mapper = mapper;
    }

    public async Task<GuidIdResult> CreateWalletAsync(CreateWalletCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.Wallet wallet = this.mapper.Map<Domain.Entities.Wallet>(command);
        return new GuidIdResult(await this.walletRepository.CreateAsync(wallet));
    }

    public async Task<WalletResponse> GetWalletAsync(Guid userId, CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Entities.Wallet> wallets = await this.walletRepository.GetByUserIdAsync(userId);
        if (!wallets.Any())
        {
            throw new WalletException(ErrorCode.WalletNotFound,
                ErrorCode.WalletNotFound.GetDescription(), HttpStatusCode.NotFound);
        }

        return new WalletResponse()
        {
            Items = this.mapper.Map<List<WalletResponseItem>>(wallets),
        };
    }

    public async Task UpdateWalletAsync(CreateWalletTransactionCommand command, ITransactionScope? transactionScope)
    {
        Domain.Entities.Wallet wallet = await this.walletRepository.GetAsync(command.WalletId);
        if (wallet is null)
        {
            throw new WalletException(ErrorCode.WalletNotFound,ErrorCode.WalletNotFound.GetDescription(), HttpStatusCode.NotFound);
        }

        CurrencyValidation(wallet.CurrencyCode, command.CurrencyCode);
        if (command.TransactionType == TransactionType.Deposit)
        {
            wallet.Limit += command.Amount;
        }
        else
        {
            if (wallet.Limit < command.Amount)
            {
                throw new WalletException(ErrorCode.LimitInsufficent,ErrorCode.LimitInsufficent.GetDescription(), HttpStatusCode.BadRequest); 
            }

            wallet.Limit -= command.Amount;
        }

        await this.walletRepository.UpdateAsync(wallet, transactionScope);
    }

    private void CurrencyValidation(CurrencyCode walletCurrency, CurrencyCode walletTransactionCurrency)
    {
        if (walletCurrency != walletTransactionCurrency)
        {
            throw new WalletException(ErrorCode.WalletCurrencyNotMatched,ErrorCode.WalletCurrencyNotMatched.GetDescription(), HttpStatusCode.BadRequest); 
        }
    }
}