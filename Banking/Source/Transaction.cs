using CsvHelper.Configuration;

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
    }
}