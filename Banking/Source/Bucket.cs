using System.Collections.Generic;
using System.Linq;
using Source;

namespace Banking.Source
{
    public class Bucket
    {
        public Bucket(string name)
        {
            Name = name;
        }

        public void AddTransaction(Transaction transaction)
        {
            if(transaction != null)
                Transactions.Add(transaction);
        }
        
        public string Name { get; }
        public readonly List<Transaction> Transactions = new List<Transaction>();

        public List<CategorisedTransaction> CategoriseBuckets()
        {
            if (Transactions == null || !Transactions.Any())
                return new List<CategorisedTransaction>();
            
            return Transactions.Select(trans => new CategorisedTransaction(trans, Name))
                .ToList();
        }
    }

    public class OutBucket
    {
        public string Name { get; set; }
        public decimal Total { get; set; }
    }

    public class BucketName
    {
        public string Name { get; set; }
    }
}