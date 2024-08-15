namespace Web_API_Data_Server.Models.Transaction
{
    public class Transaction
    {
        private uint accountNo;
        private decimal amount;
        private string transactionType;
        private DateTime transactionDate;

        public uint AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }
    }
}
