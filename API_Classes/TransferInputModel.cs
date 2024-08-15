namespace API_Classes
{
    public class TransferInputModel
    {
        public uint SenderAccountNo { get; set; }
        public uint RecipientAccountNo { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
