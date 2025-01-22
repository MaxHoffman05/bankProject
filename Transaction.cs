using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BankProject
{
    internal class Transaction
    {
        //Transaction Type
        public enum TransactionType  {
            None,
            Deposit,
            Withdraw,
            Transfer
        }
        //Transaction Type
        public TransactionType Type { get; private set; }
        //Account Number we are preforming for
        public  int AccountNumber { get; private set; }
        //Account Number we are preforming on (only for transfers currently)
        public int TargetAccountNumber { get; private set; }
        //Amount for the transaction
        public decimal Amount { get; private set; }


        //constructors
        public Transaction(char type, int accNum, int targetNumber, decimal amount) {
            Type = getType(type);
            AccountNumber = accNum;
            TargetAccountNumber = targetNumber;
            Amount = amount;
        }
        public Transaction(char type, int accNum, decimal amount) {
            Type = getType(type);
            AccountNumber = accNum;
            TargetAccountNumber = -1;//Not transferring money
            Amount = amount;
        }

        //convert to enum
        private TransactionType getType(char type)
        {
            if (type.Equals('d'))
            {
                return TransactionType.Deposit;
            }
            else if (type.Equals('w'))
            {
                return TransactionType.Withdraw;
            }
            else if (type.Equals('t'))
            {
                return TransactionType.Transfer;
            }
            else
            {
                return TransactionType.None;
            }
        }

        public override string ToString()
        {
            return $"Transaction for {AccountNumber} : Type - {Type}, Acting on - {TargetAccountNumber}, For Amount - {Amount:C}";
        }
    }
}
