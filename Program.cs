using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankProject;

namespace BankProject
{
    internal class Program
    {

        public static void menu() {
            Console.WriteLine("Welcome to the Bank System");
            Console.WriteLine("1. View All Accounts");
            Console.WriteLine("2. Start Transaction Simulation");
            Console.WriteLine("3. Generate Reports");
            Console.WriteLine("4. Exit");
            Console.WriteLine();
        }
        public static void reportsMenu() {
            Console.WriteLine("Generate Reports: ");
            Console.WriteLine("1. Accounts with Balance Below X dollar amount");
            Console.WriteLine("2. Account with Highest Balance");
            Console.WriteLine("3. Account with Lowest Balance");
            Console.WriteLine("4. Average Balance");
            Console.WriteLine("5. Median Balance");
            Console.WriteLine("6. Back");
            Console.WriteLine();
        }
        public static Bank createAccounts(int totalNumberOfAccounts)
        {
            //names list for dummy data
            List<string> names = new List<string> {
                "Emily", "Riley", "Leah", "Emma", "Isaac", "Alexander", "James", "Zoe", "Josephine",
                "Aria", "Joseph", "Elijah", "Asher", "John", "Addison", "Charlotte", "Ryan", "Samuel",
                "Noah", "Max", "Harper", "Levi", "Maya", "Hudson", "Victor", "Evelyn", "Madison",
                "Ruby", "Caleb", "Sarah", "Tristan", "Gabriel", "Henry", "Abigail", "Olivia", "Owen",
                "Carson", "Tyler", "Benjamin", "Jacob", "Hannah", "Ellie", "William", "Matthew", "Zara",
                "Ella", "Sophia", "Michael", "Jasper", "Mason", "Addison", "Noah", "Amelia", "Lily",
                "Lucas", "Mila", "Madeline", "Jack", "Liam", "Oliver", "Luke", "Lena", "Aiden",
                "Luca", "Scarlett", "Nora", "Levi", "Leo", "Ava", "Zara", "Ellie", "David", "Charlie",
                "Mia", "Lucas", "Ethan", "Maya", "Sophia", "Victoria", "Eli", "Jace", "Harper",
                "Avery", "Aidan", "Grace", "Chloe", "Sophie", "Dylan", "Elliot", "Sophia", "Riley",
                "Eden", "Theo", "Mason", "Hannah", "Madeline", "Oscar", "Evan", "Lydia", "Leah",
                "Quinn", "Olivia", "Carter", "Archer", "Zoey", "Zane", "Eli", "Liam", "Kayla",
                "Derek", "Abel", "Leo", "Amos", "Sebastian", "Nina", "Layla", "Sadie", "Jaden"
            };


            //create starter accounts
            Bank bank = new Bank();
            //randomly create x amount of accounts
            Random rnd = new Random();
            
            

            //create random amount of accounts for bank
            for (int i = 1; i <= totalNumberOfAccounts; i++)
            {
                //creates an account with account number i, a psuedo random name where the first name is pulled from the names list
                //then a last name is pulled from the first character in another name in the list. The name doesn't have to be unique
                //but I wanted to have it somewhat unique just to illustrate what is happening to which accounts
                int cap = rnd.Next(0, 10000);
                bank.addAccount(i, names[i] + " " + names[rnd.Next(0, 100)][0], rnd.Next(0, cap));
            }
            return bank;
        }

        static void Main(string[] args)
        {

            Bank bank = new Bank();
            Console.WriteLine("Enter total possible number of accounts, (100 max)");
            int totalNumberOfAccounts = Convert.ToInt32(Console.ReadLine());
            bank = createAccounts(totalNumberOfAccounts);

            
            Console.WriteLine("All accounts");
            bank.ListAccounts();
            Console.WriteLine();



            bool isRunning = true;
            do//program loop
            {
                
                menu();
                string choice = Console.ReadLine();
                choice = choice.Trim();
                switch (choice)//read in and check user input
                {
                    case "1":
                        bank.ListAccounts();
                        break;
                    case "2":
                        //Here is going to be the threads and simulating some transactions
                        Console.WriteLine("Transaction simulation started...");
                        bank.ListAccounts();

                        //we will preform a number of transactions across all the accounts
                        //the number of total transactions will be 1/4 of total number of accounts
                        int numOfTransactions = totalNumberOfAccounts / 4;

                        //transaction queue
                        ConcurrentQueue<Transaction> queue = new ConcurrentQueue<Transaction>();
                        queue = CreateTransactionQueue(numOfTransactions, totalNumberOfAccounts);
                        Console.WriteLine("-----------Starting Simulation-----------");
                        Console.WriteLine($"Preforming {numOfTransactions} number of transactions");

                        //start our threads, chat GPT recommended using a for loop and array to create and manage our 4 tasks
                        Task[] tasks = new Task[4];
                        for(int i = 0; i < 4; i++)
                        {
                            
                            int task = i;
                            //used for debugging if you want to see when they each start
                            //Console.WriteLine($"Task {task} start");
                            //Console.WriteLine($"Queue length before task start: {queue.Count}");
                            tasks[i] = Task.Run(() => simulationLoop(queue, bank));
                           // Console.WriteLine($"Task {task} end");
                        }
                        Task.WhenAll(tasks).Wait();
                        


                        Console.WriteLine("-----------Finished Simulation-----------");
                        Console.WriteLine();
                        Console.WriteLine("Current state of accounts");
                        bank.ListAccounts();
                        break;
                    case "3":
                        //Generate Reports and run LINQ queries
                        bool reportsRun = true;
                        do
                        {
                            
                            //prints out menu for reports
                            reportsMenu();
                            choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":
                                    Console.WriteLine("Enter a dollar amount");
                                    string amount = Console.ReadLine();
                                    decimal number = decimal.Parse(amount);
                                    bank.belowAmount(number);
                                    break;
                                case "2":
                                    bank.highestBalance();
                                    break;
                                case "3":
                                    bank.lowestBalance();
                                    break;
                                case "4":
                                    bank.averageBalance();
                                    break;
                                case "5":
                                    bank.medianBalance();
                                    break;
                                case "6":
                                    reportsRun = false;
                                    break;
                            }


                        } while (reportsRun);
                        reportsRun = true;
                        break;
                    case "4":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Enter a number 1-4");
                        break;
                }

            } while (isRunning);
        }


        public static ConcurrentQueue<Transaction> CreateTransactionQueue(int numOfTransactions, int totalNumberOfAccounts)
        {
            ConcurrentQueue<Transaction> queue = new ConcurrentQueue<Transaction>();
            Random rnd = new Random();
            for (int i = 0; i < numOfTransactions; i++)
            {
                int transactionNum = rnd.Next(1, 4);//determines action
                int accountNum = rnd.Next(1, totalNumberOfAccounts);//account number to preform transaction on
                decimal dollarAmount = rnd.Next(1, 200);//random amount we will do a transaction for
                                                          //get our type
                char type = 'n';
                switch (transactionNum)
                {
                    case 1:
                        //deposit
                        type = 'd';
                        break;
                    case 2:
                        //withdraw
                        type = 'w';
                        break;
                    case 3:
                        //transfer
                        type = 't';
                        break;

                }

                //create our transactions
                Transaction newTransaction;
                if (type == 't')//if its a transfer we also need to set the targetNum to a target Account Number
                {
                    int targetNum = rnd.Next(1, totalNumberOfAccounts);
                    newTransaction = new Transaction(type, accountNum, targetNum, dollarAmount);
                }
                else
                {//this constructor is for withdraws and deposit transactions
                    newTransaction = new Transaction(type, accountNum, dollarAmount);
                }

                //add transaction to the queue
                queue.Enqueue(newTransaction);
            }
            return queue;
        }


        public static void simulationLoop(ConcurrentQueue<Transaction> queue, Bank bank)
        {
            
            //loop over all transactions
            while (!queue.IsEmpty)
            {
                Transaction ts;

                if (queue.TryDequeue(out ts))//try to get and remove the first transaction
                {
                    Console.WriteLine($"Task {Task.CurrentId} - working on transaction: {ts}");
                    //get our transaction data
                    Transaction.TransactionType type = ts.Type;
                    int accountNumber = ts.AccountNumber;
                    int targetAcc = ts.TargetAccountNumber;
                    decimal amount = ts.Amount;
                    //preform the transaction based on the enum TransactionType which tells us which function to call
                    if (type == Transaction.TransactionType.Deposit)
                    {
                        
                        bank.DepositToAccount(accountNumber, amount);
                    }
                    else if (type == Transaction.TransactionType.Withdraw)
                    {
                        
                        bank.WithdrawFromAccount(accountNumber, amount);
                    }
                    else if (type == Transaction.TransactionType.Transfer)
                    {
                        
                        bank.TransferToAccount(accountNumber, targetAcc, amount);
                    }
                    else
                    {
                        Console.WriteLine("Error not a valid transaction, you should never see this message");
                    }


                    //Thread.Sleep(100);
                }

            }
        }

    }
}