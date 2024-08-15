using System;
using System.Windows;
using System.Windows.Controls;
using RestSharp;
using Newtonsoft.Json;
using API_Classes;

namespace Desktop
{
    public partial class MainWindow : Window
    {
        private readonly RestClient client;

        public MainWindow()
        {
            InitializeComponent();

            client = new RestClient("http://localhost:5013");
            /*IndexBox.Text = GetNumEntries().ToString();*/
        }

        /*private int GetNumEntries()
        {
            var request = new RestRequest("api/accounts/count");
            var response = client.Get(request);
            return int.TryParse(response.Content, out int count) ? count : 0;
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint accountNo = uint.TryParse(IndexBox.Text, out uint result) ? result : 0;
                if (accountNo == 0)
                {
                    throw new Exception("Invalid Account Number Entered");
                }

                var request = new RestRequest($"api/accountno/{accountNo}");
                var response = client.Get(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"API returned an error. Status: {response.StatusCode}. Message: {response.Content}");
                }

                var dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(response.Content);

                fNameBox.Text = dataIntermed.FirstName;
                lNameBox.Text = dataIntermed.LastName;
                balanceBox.Text = dataIntermed.Balance.ToString("C");
                accNoBox.Text = dataIntermed.AccountNo.ToString();
                pinBox.Text = dataIntermed.PIN.ToString("D4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void createAccountButton_Click(object sender, RoutedEventArgs e)
        {
            BankAccountCreation bankAccountCreationWindow = new BankAccountCreation();
            bankAccountCreationWindow.Show();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint accountNo = uint.Parse(IndexBox.Text);
                DataIntermed updatedAccount = new DataIntermed
                {
                    AccountNo = accountNo,
                    FirstName = fNameBox.Text,
                    LastName = lNameBox.Text,
                    PIN = Convert.ToUInt32(pinBox.Text),
                    Balance = Convert.ToInt32(balanceBox.Text)
                };

                var client = new RestClient("http://localhost:5013");
                var request = new RestRequest($"api/accounts/{accountNo}", Method.Put);
                request.AddJsonBody(updatedAccount);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Account successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Failed to update account. Error: {response.Content}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint accountNo = uint.Parse(IndexBox.Text);

                var client = new RestClient("http://localhost:5013"); // Replace with the URL of your Business Tier API
                var request = new RestRequest($"api/accounts/{accountNo}", Method.Delete);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Account successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Failed to delete account. Error: {response.Content}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}