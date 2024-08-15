using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankBusinessTier
{
    [ServiceContract]
    public interface BankBusinessInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        void GetValuesForSearch(string searchText, out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance);
        [OperationContract]
        void GetValuesForEntry(int index, out uint pin, out uint acctNo, out string firstName, out string
lastName, out int balance);
    }
}


