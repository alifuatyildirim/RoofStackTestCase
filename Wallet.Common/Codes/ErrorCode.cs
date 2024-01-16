using System.ComponentModel;

namespace Wallet.Common.Codes;

public enum ErrorCode
{
    [Description("Bir hata oluştu.")]
    GenericError = 1000,

    [Description("Geçersiz adres.")]
    InvalidPath = 1001,

    [Description("Geçersiz kullanıcı adı ve şifre.")]
    InvalidUsernameAndPassword = 1002,

    [Description("Yetkisiz işlem.")]
    Unauthorized = 1003,

    [Description("Geçersiz kullanıcı adı.")]
    InvalidUsername = 1004,

    [Description("İş kuralı hatası")]
    BusinessRuleError = 1005,

    [Description("{0}")]
    InvalidRequest = 1008,
    
    [Description("Cüzdanın para birimi eşleşmiyor")]
    WalletCurrencyNotMatched = 1009,
    
    [Description("Cüzdan bulunamadı")]
    WalletNotFound = 1010,
    
    [Description("Limit yetersiz")]
    LimitInsufficent = 1011,
    
    [Description("Cüzdan bilgisi boş olmamalı")]
    WalletIdCannotBeEmpty = 1012,
    
    [Description("İşlem yapan kullanıcı bilgisi boş olmamalı")]
    CreatedByCannotBeEmpty = 1013,
    
    [Description("İşlem miktarı sıfır(0) dan büyük olmalı")]
    AmountGreatherThanZero = 1013,
    
    [Description("Hatalı para birimi")]
    InvalidCurrencyCode = 1014,
}