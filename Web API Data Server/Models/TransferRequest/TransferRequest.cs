namespace Web_API_Data_Server.Models.TransferRequest
{
    public class TransferRequest
    {
        public uint SenderAccountNo { get; set; }
        public uint RecipientAccountNo { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

}
