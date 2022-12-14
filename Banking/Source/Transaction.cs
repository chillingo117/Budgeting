using System;

namespace Banking.Source
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
        public Guid Guid { get; set; }
    }
    
    public sealed class TransactionCsvMap : CsvHelper.Configuration.ClassMap<Transaction>
    {
        public TransactionCsvMap()
        {
            string format = "dd/MM/yy";
            Map(m => m.Date).Name("Date").TypeConverterOption.Format(format);
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Payee).Name("Payee");
            Map(m => m.Particulars).Name("Particulars");
            Map(m => m.Code).Name("Code");
            Map(m => m.Reference).Name("Reference");
            Map(m => m.OtherPartyAccount).Name(Constants.OtherPartyAccountColumnName);
            Map(m => m.Guid).Ignore();
        }
    }
}