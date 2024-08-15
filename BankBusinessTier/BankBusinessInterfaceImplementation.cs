using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bank_System_Server;
using BankDLL;
using System.Runtime.CompilerServices;

namespace BankBusinessTier
{
    internal class BankBusinessInterfaceImplementation : BankBusinessInterface
    {
        private uint LogNumber = 0;
        private DatabaseInterface foob;
        public BankBusinessInterfaceImplementation()
        {

            ChannelFactory<DatabaseInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/BankService";
            foobFactory = new ChannelFactory<DatabaseInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


        }
        public int GetNumEntries()
        {
            Log($"GetNumEntries called.");
            return foob.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance)
        {
            Log($"GetValuesForEntry called.");
            foob.GetValuesForEntry(index, out pin, out acctNo, out firstName, out lastName, out balance);
        }

        public void GetValuesForSearch(string searchText, out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance)
        {
            pin = 0;
            acctNo = 0;
            firstName = null;
            lastName = null;
            balance = 0;
            int numEntry = foob.GetNumEntries();
            for (int i = 1; i <= numEntry; i++)
            {
                uint sPin;
                uint sAcctNo;
                string sFirstName;
                string sLastName;
                int sBalance;
                foob.GetValuesForEntry(i, out sPin, out sAcctNo, out sFirstName, out sLastName, out sBalance);
                if (sFirstName.ToLower().Contains(searchText.ToLower()))
                {
                    pin = sPin;
                    acctNo = sAcctNo;
                    firstName = sFirstName;
                    lastName = sLastName;
                    balance = sBalance;
                    break;
                }
            }
            Log($"GetValuesForSearch called.");
            Thread.Sleep(5000); //Forced sleep for two seconds
        }

        private void Log(string logString)
        {
            LogNumber++;
            Console.WriteLine($"[{LogNumber}] - {logString}");
        }
    }
}
