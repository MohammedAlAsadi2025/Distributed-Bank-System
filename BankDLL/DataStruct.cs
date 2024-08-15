using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankDLL
{
    internal class DataStruct
    {
        public uint accountNo;
        public uint pin;
        public int balance;
        public string firstName;
        public string lastName;
        public DataStruct()
        {
            accountNo = 0;
            pin = 0;
            balance = 0;
            firstName = "";
            lastName = "";
        }
    }
}
