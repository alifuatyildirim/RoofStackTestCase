using FluentValidation;
using Wallet.Common.Codes;
using Wallet.Common.Extensions;
using Wallet.Contract.Query.Wallet;

namespace Wallet.Application.Handlers.Wallet.Validators;

public class GetWalletByUserQueryHandlerValidator: AbstractValidator<GetWalletByUserQuery>
{
    public GetWalletByUserQueryHandlerValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty().WithMessage(ErrorCode.InvalidUsername.GetDescription());
    }
}