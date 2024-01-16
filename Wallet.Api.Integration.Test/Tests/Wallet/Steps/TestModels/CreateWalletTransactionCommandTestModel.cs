using System;
using Wallet.Common.Enums;

namespace Wallet.Api.Integration.Test.Tests.Wallet.Steps.TestModels;

public class CreateWalletTransactionCommandTestModel
{
    public CurrencyCode CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid CreatedBy { get; set; }
}