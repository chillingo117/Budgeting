using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;

namespace Banking.Source
{
    public class Sorter
    {
        public readonly List<string> Buckets = Constants.Buckets;

        public List<SortedTransaction> SortedTransactions = new List<SortedTransaction>();

        public List<Transaction> Transactions = new List<Transaction>();
        public string SearchFilter { get; set; }

        private Transaction CurrentTransaction()
        {
            return String.IsNullOrWhiteSpace(SearchFilter)
                ? Transactions.FirstOrDefault()
                : GetFilteredTransactions().FirstOrDefault();
        }

        public void AddBucket(string bucketName)
        {
            if(String.IsNullOrWhiteSpace(bucketName))
                return;
            if(!Buckets.Exists(b => b == bucketName))
                Buckets.Add(bucketName);
        }

        public void RemoveBucket(string bucketName)
        {
            var bucketToRemove = Buckets.SingleOrDefault(b => b == bucketName);
            if (bucketToRemove == null)
                return;

            Transactions.InsertRange(0, SortedTransactions.Where(st => st.Bucket == bucketName));
            SortedTransactions = SortedTransactions.Where(st => st.Bucket != bucketName).ToList();

            Buckets.Remove(bucketToRemove);
        }

        public void AssignTransactionToBucket(string name)
        {
            if (CurrentTransaction() == default)
                return;
            SortedTransactions.Add(new SortedTransaction(CurrentTransaction(), name));
            Transactions.Remove(CurrentTransaction());
        }
        
        public void AssignAllTransactionsToBucket(string name)
        {
            var transactions = GetFilteredTransactions();
            if (!transactions.Any())
                return;
            transactions.ForEach(t =>
            {
                SortedTransactions.Add(new SortedTransaction(t, name));
                Transactions.Remove(t);
            });
        }

        public void UndoTransactionSort(Guid transactionId)
        {
            var transactionToUnsort = SortedTransactions.SingleOrDefault(st => st.Guid == transactionId);
            if (transactionToUnsort == default)
                return;
            SortedTransactions.Remove(transactionToUnsort);
            Transactions.Insert(0, transactionToUnsort);
        }
        
        public List<Transaction> GetFilteredTransactions()
        {
            return Transactions.Where(t => Regex.IsMatch(t.Payee, SearchFilter)).ToList();
        }

        public void LoadData(string file)
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionCsvMap>();
                Transactions = csv.GetRecords<Transaction>().ToList();
                Transactions.ForEach(t => t.Guid = Guid.NewGuid());
            }
        }
    }
}