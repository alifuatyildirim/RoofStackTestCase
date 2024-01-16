Feature: CreateWalletCommand

    Scenario: Create Wallet for a User with POST Request.Then http status should be OK
        Given Wallets are
          | UserId                               | CurrencyCode | Limit | CreatedDate |
          | 11111111-1111-1111-1111-111111111111 | TRY          | 500   | 2024-01-13  |
          | 11111111-1111-1111-1111-111111111111 | Dolar        | 100   | 2024-01-13  |
          | 22222222-2222-2222-2222-222222222222 | TRY          | 1000  | 2024-01-13  |
        When POST "/wallet/create/{11111111-1111-1111-1111-111111111111}" is called with parameters
          | currencyCode |
          | Euro         |
       Then Http status code should be 200 and Message should be "" and error code should be ""
        Then Wallets should be
          | UserId                               | CurrencyCode | Limit |
          | 11111111-1111-1111-1111-111111111111 | TRY          | 500   |
          | 11111111-1111-1111-1111-111111111111 | Dolar        | 100   |
          | 11111111-1111-1111-1111-111111111111 | Euro         | 0     |
          | 22222222-2222-2222-2222-222222222222 | TRY          | 1000  |