using System.ComponentModel;

namespace Wallet.Common.Codes;

public enum ErrorCode
{
    [Description("Bir hata oluştu.")]
    GenericError = 1000,
    
    [Description("{0}")]
    InvalidRequest = 1001,
    
    [Description("Geçersiz kullanıcı adı.")]
    InvalidUsername = 1002,
    
    [Description("Cüzdanın para birimi eşleşmiyor")]
    WalletCurrencyNotMatched = 1003,
    
    [Description("Cüzdan bulunamadı")]
    WalletNotFound = 1004,
    
    [Description("Limit yetersiz")]
    LimitInsufficent = 1005,
    
    [Description("Cüzdan bilgisi boş olmamalı")]
    WalletIdCannotBeEmpty = 1006,
    
    [Description("İşlem yapan kullanıcı bilgisi boş olmamalı")]
    CreatedByCannotBeEmpty = 1007,
    
    [Description("İşlem miktarı sıfır(0) dan büyük olmalı")]
    AmountGreatherThanZero = 1008,
    
    [Description("Hatalı para birimi")]
    InvalidCurrencyCode = 1009,
}