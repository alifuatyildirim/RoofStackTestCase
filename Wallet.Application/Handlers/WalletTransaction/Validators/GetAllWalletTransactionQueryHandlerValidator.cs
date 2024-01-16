using FluentValidation;
using Wallet.Common.Codes;
using Wallet.Common.Extensions;
using Wallet.Contract.Query.WalletTransaction;

namespace Wallet.Application.Handlers.WalletTransaction.Validators;

public class GetAllWalletTransactionQueryHandlerValidator : AbstractValidator<GetAllWalletTransactionQuery>
{
    public GetAllWalletTransactionQueryHandlerValidator()
    {
        this.RuleFor(x => x.WalletId)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.WalletIdCannotBeEmpty.GetDescription());
    }
}