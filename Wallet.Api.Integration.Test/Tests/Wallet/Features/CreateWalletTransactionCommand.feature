Feature: CreateWalletTransactionCommand

    Scenario: Create Wallet Transaction for a WalletId with POST Request.Then http status should be OK
        Given Wallets are
          | Id                                   | UserId                               | CurrencyCode | Limit | CreatedDate |
          | 99999999-9999-9999-9999-999999999990 | 11111111-1111-1111-1111-111111111111 | TRY          | 250   | 2024-01-13  |
        Given Wallet Transactions are
          | WalletId                             | CurrencyCode | Amount | TransactionType | CreatedBy                            |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 100    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 200    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | TRY          | 50     | WithDraw        | 11111111-1111-1111-1111-111111111111 |
        When POST "/walletTransaction/create/99999999-9999-9999-9999-999999999990" is called with parameters
          | CurrencyCode | Amount | TransactionType | CreatedBy                            |
          | TRY          | 100    | Deposit         | 11111111-1111-1111-1111-111111111111 |
        Then Http status code should be 200 and Message should be "" and error code should be ""
        Then Wallet By User should be for user 11111111-1111-1111-1111-111111111111
          | UserId                               | CurrencyCode | Limit |
          | 11111111-1111-1111-1111-111111111111 | TRY          | 350   |
        Then Wallet Transaction should be for wallet 99999999-9999-9999-9999-999999999990
          | WalletId                             | Amount | TransactionType | CreatedBy                            |
          | 99999999-9999-9999-9999-999999999990 | 100    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | 200    | Deposit         | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | 50     | WithDraw        | 11111111-1111-1111-1111-111111111111 |
          | 99999999-9999-9999-9999-999999999990 | 100    | Deposit         | 11111111-1111-1111-1111-111111111111 |