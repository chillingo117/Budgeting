using System;
using Banking.Source;

namespace Source
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Payee { get; set; }
        public string Particulars { get; set; }
        public string Code { get; set; }
        public string Reference { get; set; }
        public string OtherPartyAccount { get; set; }
    }
    
    public sealed class TransactionCsvMap : CsvHelper.Configuration.ClassMap<Transaction>
    {
        public TransactionCsvMap()
        {
            string format = "dd/mm/yy";
            Map(m => m.Date).Name("Date").TypeConverterOption.Format(format);
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Payee).Name("Payee");
            Map(m => m.Particulars).Name("Particulars");
            Map(m => m.Code).Name("Code");
            Map(m => m.Reference).Name("Reference");
            Map(m => m.OtherPartyAccount).Name(Constants.OtherPartyAccountColumnName);
        }
    }
    
    public class CategorisedTransaction : Transaction
    {
        public string Bucket { get; set; }
        public DateTime ModifiedOn { get; set; }

        public CategorisedTransaction(Transaction transaction, string bucketName)
        {
            Date = transaction.Date;
            ModifiedOn = DateTime.UtcNow;
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