# **Project Outline: Multi-Threaded Bank System**



#### **Objective:**
Create a console-based bank system in C# to simulate basic banking operations. This project will focus on **multi-threading**, **event-driven programming**, and **LINQ**, giving you hands-on experience with C#'s unique features.

---

#### **Features:**

1. **Customer Accounts**
   - Each account has:
     - `Name` (string): The account holder's name.
     - `AccountNumber` (int): A unique identifier for the account.
     - `Balance` (decimal): The current account balance.
   - Accounts are stored in a **shared collection** (e.g., `List<CustomerAccount>`).

2. **Transactions**
   - Implement the following operations:
     - **Deposit:** Add funds to an account.
     - **Withdraw:** Deduct funds from an account (ensure balance does not go below zero).
     - **Transfer:** Move funds from one account to another.
   - Use **multi-threading** to simulate multiple transactions occurring simultaneously.

3. **Notifications (Events and Delegates)**
   - Trigger notifications for:
     - Accounts with a balance below $50.
     - Transactions of $1,000 or more.
   - Use **events** to display these notifications in the console.

4. **Reports (LINQ Queries)**
   - Implement two reports:
     - List all accounts with balances below a specified threshold.
     - Find the account with the highest balance.

5. **Concurrency and Thread Safety**
   - Ensure thread-safe access to the shared collection using:
     - Synchronization mechanisms like `lock`.
     - Or thread-safe collections like `ConcurrentBag<CustomerAccount>`.

6. **Console UI**
   - Provide a simple menu for the user to:
     - View all accounts and their balances.
     - Start the transaction simulation.
     - Generate and view reports.
     - Exit the application.

---

#### **Implementation Guidelines:**

1. **Class Design:**
   - `CustomerAccount`: Represents an account with properties like `Name`, `AccountNumber`, and `Balance`.
   - `TransactionSimulator`: Manages transaction threads and randomly performs operations on accounts.
   - `Bank`: Contains the shared collection and handles operations like deposits, withdrawals, and report generation.

2. **Concurrency:**
   - Use `Task.Run` or `Thread` to simulate transactions.
   - Implement thread-safety for the shared collection to avoid race conditions.

3. **Event-Driven Notifications:**
   - Define events in the `Bank` class for low-balance and large-transaction notifications.
   - Subscribe to these events and display relevant messages in the console.

4. **LINQ for Reports:**
   - Use LINQ to filter and analyze the account data.

---

#### **Sample Console Flow:**
```
Welcome to the Bank System!
1. View All Accounts
2. Start Transaction Simulation
3. Generate Reports
4. Exit
```

Example Output:
```
Account: John Doe, Balance: $42.50 (Low Balance)
Transaction Alert: $1,500 deposited to Jane Smith's account.
Report: Accounts with Balance < $50:
- John Doe: $42.50
Highest Balance: Jane Smith ($2,000.00)
```

---

#### **Timeline:**
1. **Day 1:**
   - Create the `CustomerAccount` and `Bank` classes.
   - Set up the shared collection and basic account operations (add/view accounts).
   - Implement deposit, withdraw, and transfer methods.

2. **Day 2:**
   - Add multi-threading to simulate transactions.
   - Implement event-driven notifications for specific conditions.
   - Add LINQ-based reports and test the application.



# Method Outline

### **Methods to Include in the Bank Class**

The `Bank` class should manage all bank-level operations, including the shared collection of customer accounts and transaction-related features. Here's the list of methods to include:

#### 1. **Account Management**
- `AddAccount(CustomerAccount account)`: Adds a new account to the shared collection.
- `RemoveAccount(int accountNumber)`: Removes an account by its unique account number.
- `GetAccount(int accountNumber)`: Retrieves an account from the shared collection by its account number.
- `ListAccounts()`: Returns a list of all accounts for reporting or display purposes.

#### 2. **Transaction Management**
- `Deposit(int accountNumber, decimal amount)`: Deposits a specified amount into the given account.
- `Withdraw(int accountNumber, decimal amount)`: Withdraws a specified amount from the given account, ensuring the balance does not go negative.
- `Transfer(int fromAccountNumber, int toAccountNumber, decimal amount)`: Transfers funds between two accounts.

#### 3. **Event Handling**
- `OnLowBalance(CustomerAccount account)`: Triggers an event when an account balance falls below a specific threshold.
- `OnLargeTransaction(CustomerAccount account, decimal amount)`: Triggers an event for large deposits or withdrawals.

#### 4. **Multi-Threading Support**
- `SimulateTransactions()`: Starts multiple threads to simulate concurrent deposits, withdrawals, and transfers.
- `StopSimulation()`: Stops the transaction simulation (optional, for managing threads gracefully).

#### 5. **Reporting (Using LINQ)**
- `GetAccountsBelowBalance(decimal threshold)`: Returns a list of accounts with balances below the given threshold.
- `GetHighestBalanceAccount()`: Returns the account with the highest balance.

---

### **Methods to Include in the CustomerAccount Class**

The `CustomerAccount` class should represent individual bank accounts and include methods related to account-specific operations. Here�s the list:

#### 1. **Basic Operations**
- `Deposit(decimal amount)`: Adds the specified amount to the account's balance.
- `Withdraw(decimal amount)`: Deducts the specified amount from the account's balance (with validation to prevent negative balances).

#### 2. **Utility Methods**
- `HasLowBalance(decimal threshold)`: Returns `true` if the account balance is below the given threshold.
- `ToString()`: Overrides the default `ToString()` method to display account information (e.g., `Name`, `AccountNumber`, and `Balance`).

---
