using FluentValidation;
using Wallet.Common.Codes;
using Wallet.Common.Extensions;
using Wallet.Contract.Command.WalletTransaction;

namespace Wallet.Application.Handlers.WalletTransaction.Validators;

public class CreateWalletTransactionCommandHandlerValidator: AbstractValidator<CreateWalletTransactionCommand>
{
    public CreateWalletTransactionCommandHandlerValidator()
    {
        this.RuleFor(x => x.WalletId)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.WalletIdCannotBeEmpty.GetDescription());
        
        this.RuleFor(x => x.CreatedBy)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.CreatedByCannotBeEmpty.GetDescription());
        
        this.RuleFor(x => x.Amount).GreaterThan(0)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.AmountGreatherThanZero.GetDescription());
    }
}