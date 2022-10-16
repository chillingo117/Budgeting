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
        public Sorter()
        {
            if (File.Exists(Constants.BucketDataFilename))
            {
                using (var reader = new StreamReader(Constants.BucketDataFilename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Buckets = csv.GetRecords<BucketName>()
                        .Select(bn => new Bucket(bn.Name))
                        .ToList();
                }

            }
        }

        public readonly List<Bucket> Buckets = new List<Bucket>();
        public Transaction CurrentTransaction => _transactions.FirstOrDefault();
        private List<Transaction> _transactions = new List<Transaction>();

        public void AddBucket(string name)
        {
            if(name == null)
                return;
            if(!Buckets.Exists(b => b.Name == name))
                Buckets.Add(new Bucket(name));
        }

        public void RemoveBucket(string name)
        {
            var bucketToRemove = Buckets.SingleOrDefault(b => b.Name == name);
            if (bucketToRemove == null)
                return;

            _transactions.AddRange(bucketToRemove.Transactions);
            Buckets.Remove(bucketToRemove);
        }

        public void AddCurrentTransactionToBucket(string name)
        {
            Buckets.Single(b => b.Name == name).AddTransaction(CurrentTransaction);
            _transactions.Remove(CurrentTransaction);
        }

        public void LoadData(string file)
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionCsvMap>();
                _transactions = csv.GetRecords<Transaction>().ToList();
            }
        }
    }
}