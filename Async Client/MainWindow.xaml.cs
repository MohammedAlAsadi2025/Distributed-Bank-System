using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using BankBusinessTier;
using BankDLL;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Runtime.Remoting.Messaging;

namespace Async_Client
{
    public delegate Account Search(string value);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BankBusinessInterface foob;
        private Search search;
        public MainWindow()
        {
            InitializeComponent();
            //This is a factory that generates remote connections to our remote class. This is what hides the RPC stuff!
            ChannelFactory<BankBusinessInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/BankBusinessService";
            foobFactory = new ChannelFactory<BankBusinessInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            //Also, tell me how many entries are in the DB.
            IndexBox.Text = foob.GetNumEntries().ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            //On click, Get the index....
            if (!int.TryParse(IndexBox.Text, out index))
            {
                // Error parsing the index
                MessageBox.Show("Invalid index. Please enter a valid integer.");
                return;
            }
            else
            {
                index = Int32.Parse(IndexBox.Text);
                //Then, run our RPC function, using the out mode parameters...
                foob.GetValuesForEntry(index, out acct, out pin, out fName, out lName, out bal);
                //And now, set the values in the GUI!
                fNameBox.Text = fName;
                lNameBox.Text = lName;
                balanceBox.Text = bal.ToString("C");
                accNoBox.Text = acct.ToString();
                pinBox.Text = pin.ToString("D4");
            }
        }

        private void SearchButthon_Click(object sender, RoutedEventArgs e)
        {
            // Validate search text
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                MessageBox.Show("Please enter a valid search term.");
                return;
            }
            else if (SearchBox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Search term should not contain numbers.");
                return;
            }
            else if (SearchBox.Text.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)))
            {
                MessageBox.Show("Search term should not contain special characters.");
                return;
            }

            search = SearchDB;
            AsyncCallback callback;
            callback = this.OnSearchCompletion;
            IAsyncResult result = search.BeginInvoke(SearchBox.Text, callback, null);
        }

        private Account SearchDB(string value)
        {
            string fName = "";
            string lName = "";
            int bal = 0;
            uint acct = 0;
            uint pin = 0;
            foob.GetValuesForSearch(value, out pin, out acct, out fName, out lName, out bal);
            if (acct != 0)
            {
                Account aAccount = new Account();
                aAccount.FirstName = fName;
                aAccount.LastName = lName;
                aAccount.Balance = bal;
                aAccount.AccountNo = acct;
                aAccount.PIN = pin;
                return aAccount;
            }
            return null;
        }

        private void UpdateGui(Account aAccount)
        {
            fNameBox.Dispatcher.Invoke(new Action(() => fNameBox.Text = aAccount.FirstName));
            lNameBox.Dispatcher.Invoke(new Action(() => lNameBox.Text = aAccount.LastName));
            balanceBox.Dispatcher.Invoke(new Action(() => balanceBox.Text = aAccount.Balance.ToString()));
            accNoBox.Dispatcher.Invoke(new Action(() => accNoBox.Text = aAccount.AccountNo.ToString()));
            pinBox.Dispatcher.Invoke(new Action(() => pinBox.Text = aAccount.PIN.ToString()));

        }


        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            Account iAccount = null;
            Search search = null;
            AsyncResult asyncobj = (AsyncResult)asyncResult;

            if (asyncobj.EndInvokeCalled == false)
            {
                search = (Search)asyncobj.AsyncDelegate;
                iAccount = search.EndInvoke(asyncobj);

                if (iAccount == null) // Check if result from SearchDB is null
                {
                    MessageBox.Show("No match found for the provided name.");
                    return;
                }

                UpdateGui(iAccount);
            }

            asyncobj.AsyncWaitHandle.Close();
        }
    }
}

