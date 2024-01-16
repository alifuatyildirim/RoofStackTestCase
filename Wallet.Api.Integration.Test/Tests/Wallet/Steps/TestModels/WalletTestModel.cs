using System;
using Wallet.Common.Enums;

namespace Wallet.Api.Integration.Test.Tests.Wallet.Steps.TestModels;

public class WalletTestModel
{
    public Guid UserId { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
    public decimal Limit { get; set; }
}