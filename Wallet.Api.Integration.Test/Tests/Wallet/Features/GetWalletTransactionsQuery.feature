Feature: GetWalletTransactionsQuery

    Scenario: Retrieve Wallet Transaction Information for a User with GET Request
        Given Wallets are
          | Id                                   | UserId                               | CurrencyCode | Limit | CreatedDate |
          | 99999999-9999-9999-9999-999999999990 | 11111111-1111-1111-1111-111111111111 | TRY          | 250   | 2024-01-13  |
          | 99999999-9999-9999-9999-999999999991 | 11111111-1111-1111-1111-111111111111 | Dolar        | 100   | 2024-01-13  |
          | 99999999-9999-9999-9999-999999999992 | 22222222-2222-2222-2222-222222222222 | TRY          | 1000  | 2024-01-13  |
        Given Wallet Transactions are
          | WalletId                             | CurrencyCode | Amount | TransactionType | CreatedBy                            |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 100.0    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 200.0    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 50.0     | WithDraw        | 11111111-1111-1111-1111-111111111111 |
        When GET "/walletTransaction/getAll/99999999-9999-9999-9999-999999999990" is called
        Then Http status code should be 200 and Message should be "" and error code should be ""
        Then Get Wallet Transactions should be
          | WalletId                             | Amount | TransactionType | CreatedBy                            |
          | 99999999-9999-9999-9999-999999999990 | 100.0    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | 200.0    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | 50.0     | WithDraw        | 11111111-1111-1111-1111-111111111111 |