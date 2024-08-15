using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDLL
{
    public class Account
    {
        private string firstName;
        private string lastName;
        private uint pin;
        private uint accountNo;
        private int balance;
        

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; } 

        }

        public string LastName
        { 
            get { return lastName; }
            set { lastName = value; } 
        }

        public uint PIN
        {
            get { return pin; }
            set { pin = value; }
        }

        public uint AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }

        public int Balance
        { 
            get { return balance; } 
            set { balance = value; } 
        }

        
        
    }

   
}
