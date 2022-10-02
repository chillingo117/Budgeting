
using Banking.Source;
using CsvHelper.Configuration.Attributes;

namespace Source
{
    public class Transaction
    {
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string Payee { get; set; }
        public string Particulars { get; set; }
        public string Code { get; set; }
        public string Reference { get; set; }
        
        [Name("Other Party Account")]
        public string OtherPartyAccount { get; set; }
    }
    
    public class CategorisedTransaction : Transaction
    {
        public string Bucket { get; set; }

        public CategorisedTransaction(Transaction transaction, string bucketName)
        {
            Date = transaction.Date;
            Amount = transaction.Amount;
            Payee = transaction.Payee;
            Particulars = transaction.Particulars;
            Code = transaction.Code;
            Reference = transaction.Reference;
            OtherPartyAccount = transaction.OtherPartyAccount;
            Bucket = bucketName;
        }
    }
}