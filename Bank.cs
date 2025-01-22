using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject
{
    internal class Bank
    {
        //we want to use a thread safe collection, I chose to use a ConcurrentDictonary since we are working with unique account numbers
        //Format we are using the account number (int) for our key and the data we are storing is a pair of the name of the account holder (string) and the balance of the account (double)
        private ConcurrentDictionary<int, CustomerAccount> Accounts;

        //number of accounts in bank
        public int numOfAccounts { get; set; }


        //constructors
        public Bank() {
            Accounts = new ConcurrentDictionary<int, CustomerAccount>();
            numOfAccounts = 0;
        }
        //methods
        //addAccount - Add New Account, will not add duplicate accounts based on the TryAdd() method
        public void addAccount(int accountNumber, string name, decimal bal)
        {
            //There is no 'this' keyword in C# like C++
            CustomerAccount accToAdd = new CustomerAccount(accountNumber, name, bal);
            bool success = Accounts.TryAdd(accToAdd.AccountNumber, accToAdd);
            if (success) {
                numOfAccounts++;
            }
            else
            {
                Console.WriteLine("Error Adding Account to Bank System, Try Again");
            }
        }

        //RemoveAccount - Remove the account from the collection
        public void removeAccount(CustomerAccount accToRemove)
        {

            bool success = Accounts.TryRemove(accToRemove.AccountNumber, out accToRemove);
            if (success)
            {
                Console.WriteLine($"Account number : {accToRemove.AccountNumber} deleted successfully");
                numOfAccounts--;
            }
            else
            {
                Console.WriteLine($"Unable to Delete Account number : {accToRemove.AccountNumber}, Did you add this account to the Bank?");
            }
        }

        //getAccount(accNum) - Retrieves an account from the shared collection by its account number.
        public CustomerAccount GetAccount(int accountNumber)
        {
            if (Accounts.ContainsKey(accountNumber))//if it exists
            {
                CustomerAccount acc = Accounts[accountNumber];//get account from dictionary
                return acc;
            }
            else
            {
                Console.WriteLine($"Account number {accountNumber} does not exist.");//return null, the account doesnt exist
                return null;
            }
        }
        //ListAccounts() - Shows all accounts
        public void ListAccounts()
        {
            Console.WriteLine("~~~~~~~~All Accounts In Bank~~~~~~~~");
            Console.WriteLine("Total Number of Accounts : " + numOfAccounts);
            foreach (KeyValuePair<int, CustomerAccount> record in Accounts)
            {
                Console.WriteLine(record.Value);
            }
        }

        //DepositToAccount(accNumber, amount) - deposit amount into account
        public void DepositToAccount(int accountNumber, decimal amount)
        {

            if (Accounts.ContainsKey(accountNumber))
            {
                //first find the account in Accounts and create account var
                CustomerAccount accToDep = Accounts[accountNumber];
                accToDep.Deposit(amount);
                Console.WriteLine($"Depositing {amount:C} into account # {accountNumber}");
                
            }
            else
            {
                Console.WriteLine($"Account number {accountNumber} does not exist.");
                
            }
        }
        //WithdrawFromAccount(accNumber, amount) - withdraws amount from account
        public void WithdrawFromAccount(int accountNumber, decimal amount)
        {
            if (Accounts.ContainsKey(accountNumber))
            {
                //first find the account in Accounts and create account var
                CustomerAccount accToWith = Accounts[accountNumber];
                //check balance first, to make sure they have the money
                if (accToWith.Balance < amount)
                {
                    Console.WriteLine($"Unable to withdraw {amount:C} from {accountNumber}, funds too low");
                    
                }
                else
                {
                    accToWith.Withdraw(amount);
                    Console.WriteLine($"Withdrawing {amount:C} from account # {accountNumber}");
                    
                }
            }
            else
            {
                Console.WriteLine($"Account number {accountNumber} does not exist.");
                
            }
        }
        public void TransferToAccount(int senderAccNum, int recieverAccNum, decimal amount)
        {
            
            CustomerAccount goingFrom = Accounts[senderAccNum];
            CustomerAccount goingTo = Accounts[recieverAccNum];
            if(goingFrom.Balance < amount)
            {
                Console.WriteLine($"Unable to withdraw {amount} from {senderAccNum}");
                return;
            }
            else
            {

                goingFrom.Withdraw(amount);
                goingTo.Deposit(amount);
                Console.WriteLine($"Transfering {amount:C} from account # - {senderAccNum} to account # - {recieverAccNum}");

            }


        }

        public bool HasAccount(int accountNumber)
        {
            return Accounts.ContainsKey(accountNumber) ? true : false;
        }


        //LINQ Queries
        public void belowAmount(decimal amount)
        {
            //LINQ
            Console.WriteLine($"All accounts below {amount:C}");
            var output =
                from a in Accounts
                where a.Value.Balance < amount
                select new { a.Value.AccountNumber, a.Value.Balance };//We want to show the account number and balance

            foreach (var i in output)
            {
                Console.WriteLine($"Account Number : {i.AccountNumber}, Balance : {i.Balance:C}");
            }
        }

        public void highestBalance()
        {
            Console.WriteLine("Highest balance account");
            //first get all the balances in ascending order
            IEnumerable<decimal> ordered =
                from a in Accounts
                orderby a.Value.Balance
                select a.Value.Balance;
            //get the max
            var max1 = ordered.Max();
            //in case there are multiple accounts we use another query to match the max to every other balance, this also allows us to get the account number
            var output = from a in Accounts
                         where a.Value.Balance == max1
                         select new { a.Value.AccountNumber, a.Value.Balance };

            //even though this would be rare we print each out matching the max balance variable in case there are multiple
            foreach (var i in output)
            {
                //print out the account information
                Console.WriteLine($"Account Number : {i.AccountNumber}, Balance : {i.Balance:C}");
            }
        }
        public void lowestBalance()
        {
            //Same system but now for lowest except we are sorting descending to get the min
            Console.WriteLine("Lowest balance account");
            IEnumerable<decimal> ordered =
                from a in Accounts
                orderby a.Value.Balance descending
                select a.Value.Balance;
            var lowBal = ordered.Min();
            var output =
                from a in Accounts
                where a.Value.Balance == lowBal
                select new { a.Value.AccountNumber, a.Value.Balance };
            foreach (var i in output)
            {
                //print out the account information
                Console.WriteLine($"Account Number : {i.AccountNumber}, Balance : {i.Balance:C}");
            }
        }
        public void averageBalance()
        {
            var avg =
                from a in Accounts
                select a.Value.Balance;
            var output = avg.Average();
            Console.WriteLine($"Average balance for all accounts: {output:C}");
        }
        public void medianBalance()
        {
            //LINQ query to get all out customerAccount's ordered by balance
            var ordered =
               from a in Accounts
               orderby a.Value.Balance ascending
               select a.Value;//ChatGPT pointed out you can select the whole class object, that is very helpful here
            List<CustomerAccount> list = ordered.ToList();//convert to list
            
            //we will have to pick between two numbers if even
            if (numOfAccounts % 2 == 0)
            {
                //this confused me but these formulas are different because of starting at 0 in a list
                int index1 = numOfAccounts / 2;
                int index2 = (numOfAccounts / 2) - 1;

                //find middle of the two balances
                CustomerAccount acc1 = list[index1];
                CustomerAccount acc2 = list[index2];
                decimal median = (acc1.Balance + acc2.Balance) / 2;//calc median
                //print info out
                Console.WriteLine("Median Accounts");
                Console.WriteLine($"Account 1 : Account Number - {acc1.AccountNumber}, Balance - {acc1.Balance}");
                Console.WriteLine($"Account 2 : Account Number - {acc2.AccountNumber}, Balance - {acc2.Balance}");
                Console.WriteLine($"Making the Median balance : {median:C}");
                return;
            }
            else
            {
                //odd so we just pick the middle
                int index = numOfAccounts / 2;
                for (int i = 0; i < numOfAccounts; i++)
                {
                    if(i == index)
                    {
                        Console.WriteLine($"Median Account (by balance) : Account Number - {list[i].AccountNumber}, Balance - {list[i].Balance}");
                        return;
                    }
                }
            }
        }
        //internal CustomerAccount class so main can only call Bank's public methods
        internal class CustomerAccount
        {
            //Getters and Setters for our instance variables
            public string Name { get; set; }
            public int AccountNumber { get; set; }
            public decimal Balance { get; set; }
            //constructors
            public CustomerAccount(int accNum, string name, decimal bal)
            {
                AccountNumber = accNum;
                Name = name;
                Balance = bal;
            }

            private readonly object resourceLock = new object();
            //deposit(toDeposit) - adds amount to customers balance
            internal void Deposit(decimal toDeposit)
            {
                lock (resourceLock)
                {
                    Balance += toDeposit;
                }
            }

            //withdraw(toWithDraw) - subtracts amount from customers balance
            internal void Withdraw(decimal toWithdraw)
            {
                lock (resourceLock)
                {
                    Balance -= toWithdraw;
                }
            }

            //ToString() - Override to display account info
            public override string ToString()
            {
                return $"Account Number {AccountNumber} : Customer - {Name}, Balance - {Balance:C}";
            }




        }

    }
}
