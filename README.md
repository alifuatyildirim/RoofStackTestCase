# Wallet Service

## Table of Contents
1. [Summary](#summary)
2. [Details](#details)
3. [Solution](#solution)
   - [System Design](#system-design)
   - [Technologies Used](#technologies-used)
   - [Setting Up the Project](#setting-up-the-project)
   - [Running Tests](#running-tests)
4. [Usage](#usage)
   - [Creating a Wallet](#creating-a-wallet)
   - [Fetching Wallet Details](#fetching-wallet-details)
   - [Creating a Wallet Transaction](#creating-a-wallet-transaction)
   - [Fetching All Wallet Transactions](#fetching-all-wallet-transactions)

## Summary
This project is a wallet application designed to integrate with a payment system. Users can create wallets supporting different currencies, perform deposit/withdraw operations, query their current balance, and receive transaction reports. The system ensures transaction integrity, rolling back any operation in case of encountered problems during deposit/withdraw operations.

## Details
1. Users can create a wallet for a specific user.
2. Each wallet is associated with a user identified by `{userId}`.
3. Users can perform transactions on a specific wallet identified by `{walletId}`.
4. The application supports creating and fetching wallet transactions.

## Solution

### System Design
The application is built on the .NET Core platform, using MongoDB as the database. Docker-compose is utilized for easy deployment and scalability.

![System Diagram](system_diagram.png)

### Technologies Used
- .NET Core
- MongoDB
- Docker

### Setting Up the Project
1. Clone the repository: `git clone https://github.com/your-username/wallet-service.git`
2. Navigate to the project directory: `cd wallet-service`
3. Build and run the Docker containers: `docker-compose up -d`

### Running Tests
1. Ensure the project dependencies are installed: `dotnet restore`
2. Run integration tests: `dotnet test WalletService.Tests.Integration`
3. Run unit tests: `dotnet test WalletService.Tests.Unit`

## Usage

### Creating a Wallet
To create a wallet for a user, make a POST request to the following endpoint:

```
POST /wallet/create/{userId}
```

Include any necessary user or wallet details in the request body.

### Fetching Wallet Details
To fetch details of a user's wallet, make a GET request to the following endpoint:

```
GET /wallet/{userId}
```

### Creating a Wallet Transaction
To create a wallet transaction, make a POST request to the following endpoint:

```
POST /walletTransaction/create/{walletId}
```

Include the transaction details in the request body.

### Fetching All Wallet Transactions
To fetch all transactions of a specific wallet, make a GET request to the following endpoint:

```
GET /walletTransaction/getAll/{walletId}
```

---

Feel free to explore the codebase and customize it according to your needs. If you encounter any issues or have questions, please refer to the [GitHub repository](https://github.com/your-username/wallet-service) for documentation and support.
