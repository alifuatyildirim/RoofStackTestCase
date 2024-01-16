using FluentValidation;
using Wallet.Common.Codes;
using Wallet.Common.Extensions;
using Wallet.Contract.Command.Wallet;

namespace Wallet.Application.Handlers.Wallet.Validators;

public class CreateWalletCommandHandlerValidator: AbstractValidator< CreateWalletCommand>
{
    public CreateWalletCommandHandlerValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.InvalidUsername.GetDescription());

        this.RuleFor(x => x.CurrencyCode).IsInEnum().NotEmpty().WithMessage(ErrorCode.InvalidCurrencyCode.GetDescription());
    }
}