Feature: GetWalletByUserQuery.Validation

    Scenario: Retrieve Wallet Information for a User with GET Request 
        When GET "/wallet/{11111111-1111-1111-1111-111111111111}" is called
        Then Http status code should be 404 and Message should be "Cüzdan bulunamadı" and error code should be "E1004" 