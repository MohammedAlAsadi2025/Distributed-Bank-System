using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankDLL
{
    public class DatabaseClass
    {
        List<DataStruct> dataStruct;

        public DatabaseClass()
        {
            dataStruct = new List<DataStruct>();
        }

        public uint GetAcctNoByIndex(int index)
        {
            return dataStruct[index].accountNo;
        }

        public uint GetPINByIndex(int index)
        {
            return dataStruct[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return dataStruct[index].firstName;
        }

        public string GetLastNameByIndex(int index)
        {
           return dataStruct[index].lastName;
        }

        public int GetBalanceByIndex(int index)
        {
            return dataStruct[index].balance;
        }

        public int GetNumRecords()
        {
            return dataStruct.Count;
        }
    }
}
