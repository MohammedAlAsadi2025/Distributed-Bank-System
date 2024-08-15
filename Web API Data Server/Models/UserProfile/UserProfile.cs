namespace Web_API_Data_Server.Models.UserProfile
{
    public class UserProfile
    {
        private string userName;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private string phone;
        private uint accountNo;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public uint AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }
    }
}
