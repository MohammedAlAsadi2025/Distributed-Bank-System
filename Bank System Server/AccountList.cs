using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDLL;

namespace Bank_System_Server
{
    public class AccountList
    {
        public static List<Account> Accounts()
        {
            List<Account> alist = new List<Account>();

            Account account1 = new Account();
            account1.FirstName = "Robert";
            account1.LastName = "James";
            account1.PIN = 1011;
            account1.AccountNo = 0001;
            account1.Balance = 0;

            Account account2 = new Account();
            account2.FirstName = "Mia";
            account2.LastName = "Williams";
            account2.PIN = 2033;
            account2.AccountNo = 0002;
            account2.Balance = 1000;

            Account account3 = new Account();
            account3.FirstName = "Adam";
            account3.LastName = "Smith";
            account3.PIN = 4218;
            account3.AccountNo = 0003;
            account3.Balance = 2000;

            alist.Add(account1);
            alist.Add(account2);
            alist.Add(account3);

            return alist;


        }
    }
}
