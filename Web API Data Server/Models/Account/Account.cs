using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_API_Data_Server.Models.Account
{
    public class Account
    {
        private string firstName;
        private string lastName;
        private uint pin;
        private uint accountNo;
        private decimal balance;
        

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

        public decimal Balance
        { 
            get { return balance; } 
            set { balance = value; } 
        }       
    } 
}
