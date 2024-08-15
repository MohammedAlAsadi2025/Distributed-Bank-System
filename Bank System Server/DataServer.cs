using Bank_System_Server;
using BankDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bank_System_Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DatabaseInterface
    {
        public int GetNumEntries()
        {
            return AccountList.Accounts().Count;
        }

        public void GetValuesForEntry(int index, out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance)
        {
            List<Account> alist = AccountList.Accounts();
            firstName = alist[index - 1].FirstName;
            lastName = alist[index - 1].LastName;
            acctNo = alist[index - 1].AccountNo;
            pin = alist[index - 1].PIN;
            balance = alist[index - 1].Balance;
        }
    }
}
