using System;
using Banking.Source;

namespace Source
{
    public class SortedTransaction : Transaction
    {
        public string Bucket { get; set; }
        public DateTime ModifiedOn { get; set; }

        public SortedTransaction(Transaction transaction, string bucketName)
        {
            Date = transaction.Date;
            ModifiedOn = DateTime.UtcNow;
            Amount = transaction.Amount;
            Payee = transaction.Payee;
            Particulars = transaction.Particulars;
            Code = transaction.Code;
            Reference = transaction.Reference;
            OtherPartyAccount = transaction.OtherPartyAccount;
            Guid = transaction.Guid;
            Bucket = bucketName;
        }
    }
    
    public sealed class SortedTransactionCsvMap : CsvHelper.Configuration.ClassMap<SortedTransaction>
    {
        public SortedTransactionCsvMap()
        {
            string format = "dd/MM/yyyy HH:mm:ss";
            Map(m => m.Date).Name("Date").TypeConverterOption.Format(format);
            Map(m => m.ModifiedOn).Name("Modified On").TypeConverterOption.Format(format);
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Payee).Name("Payee");
            Map(m => m.Particulars).Name("Particulars");
            Map(m => m.Code).Name("Code");
            Map(m => m.Reference).Name("Reference");
            Map(m => m.OtherPartyAccount).Name(Constants.OtherPartyAccountColumnName);
            Map(m => m.Bucket).Name("Bucket");
            Map(m => m.Guid).Name("Guid");
        }
    }
}