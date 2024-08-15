using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Web_API_Data_Server.Models.Account
{
    internal class DataStruct
    {
        public uint accountNo;
        public uint pin;
        public decimal balance;
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
