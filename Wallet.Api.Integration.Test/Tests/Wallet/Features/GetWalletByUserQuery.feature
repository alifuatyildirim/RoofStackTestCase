Feature: GetWalletByUserQuery

    Scenario: Retrieve Wallet Information for a User with GET Request
        Given Wallets are
          | UserId                               | CurrencyCode | Limit | CreatedDate |
          | 11111111-1111-1111-1111-111111111111 | TRY          | 500   | 2024-01-13  |
          | 11111111-1111-1111-1111-111111111111 | Dolar        | 100   | 2024-01-13  |
          | 22222222-2222-2222-2222-222222222222 | TRY          | 1000  | 2024-01-13  |
        When GET "/wallet/{11111111-1111-1111-1111-111111111111}" is called
        Then Http status code should be 200 and Message should be "" and error code should be ""
        Then Get Wallet By User should be
          | UserId                               | CurrencyCode | Limit |
          | 11111111-1111-1111-1111-111111111111 | TRY          | 500   |
          | 11111111-1111-1111-1111-111111111111 | Dolar        | 100   |