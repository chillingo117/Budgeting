using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Source;

namespace Banking.Source
{
    public class Sorter
    {
        public class Bucket
        {
            public string Name { get; set; }
        }

        public Sorter()
        {
            if (File.Exists(Constants.BucketDataFilename))
            {
                using (var reader = new StreamReader(Constants.BucketDataFilename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Buckets = csv.GetRecords<Bucket>().ToList();
                }
            }
        }

        public Transaction CurrentTransaction => _transactions.FirstOrDefault();
        
        public readonly List<Bucket> Buckets = new List<Bucket>();

        public List<SortedTransaction> SortedTransactions = new List<SortedTransaction>();

        private List<Transaction> _transactions = new List<Transaction>();

        public void AddBucket(string bucketName)
        {
            if(String.IsNullOrWhiteSpace(bucketName))
                return;
            if(!Buckets.Exists(b => b.Name == bucketName))
                Buckets.Add(new Bucket{Name = bucketName});
        }

        public void RemoveBucket(string bucketName)
        {
            var bucketToRemove = Buckets.SingleOrDefault(b => b.Name == bucketName);
            if (bucketToRemove == null)
                return;

            _transactions.InsertRange(0, SortedTransactions.Where(st => st.Bucket == bucketName));
            SortedTransactions = SortedTransactions.Where(st => st.Bucket != bucketName).ToList();

            Buckets.Remove(bucketToRemove);
        }

        public void AssignTransactionToBucket(string name)
        {
            SortedTransactions.Add(new SortedTransaction(CurrentTransaction, name));
            _transactions.Remove(CurrentTransaction);
        }

        public void UndoTransactionSort(Guid transactionId)
        {
            var transactionToUnsort = SortedTransactions.Find(st => st.Guid == transactionId);
            SortedTransactions.Remove(transactionToUnsort);
            _transactions.Insert(0, transactionToUnsort);
        }

        public void LoadData(string file)
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionCsvMap>();
                _transactions = csv.GetRecords<Transaction>().ToList();
                _transactions.ForEach(t => t.Guid = Guid.NewGuid());
            }
        }
    }
}