using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace Bank_System_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //This should *definitely* be more descriptive.
            Console.WriteLine("hey so like welcome to my server");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(DataServer));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
            host.AddServiceEndpoint(typeof(DatabaseInterface), tcp,
            "net.tcp://0.0.0.0:8100/BankService");
        //And open the host for business!
        host.Open();
        Console.WriteLine("System Online");
        Console.ReadLine();
        //Don't forget to close the host after you're done!
        host.Close();
    }
}
}
