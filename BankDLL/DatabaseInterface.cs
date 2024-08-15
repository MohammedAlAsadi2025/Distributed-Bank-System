using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Bank_System_Server
{
    [ServiceContract]
    public interface DatabaseInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        void GetValuesForEntry(int index, out uint pin, out uint acctNo, out string firstName, out string
lastName, out int balance);
    }
}
