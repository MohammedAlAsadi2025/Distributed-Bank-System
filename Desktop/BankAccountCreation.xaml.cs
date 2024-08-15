using RestSharp;
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
using System.Windows.Shapes;
using API_Classes;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for BankAccountCreation.xaml
    /// </summary>
    public partial class BankAccountCreation : Window
    {
        public BankAccountCreation()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Collect data from the text boxes
                string firstName = fNameBox.Text;
                string lastName = lNameBox.Text;
                uint accountNo = uint.Parse(accNoBox.Text);
                uint pin = uint.Parse(pinBox.Text);
                int balance = int.Parse(balanceBox.Text);

                // Create an Account instance
                DataIntermed newAccount = new DataIntermed
                {
                    FirstName = fNameBox.Text,
                    LastName = lNameBox.Text,
                    PIN = Convert.ToUInt32(pinBox.Text),
                    AccountNo = Convert.ToUInt32(accNoBox.Text),
                    Balance = Convert.ToInt32(balanceBox.Text)
                };

                // Send the new account data to the server for creation
                var client = new RestClient("http://localhost:5013"); // Assuming you're communicating with the Business Tier
                var request = new RestRequest("api/accounts", Method.Post);
                request.AddJsonBody(newAccount);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Account successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Failed to create account. Error: {response.Content}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
