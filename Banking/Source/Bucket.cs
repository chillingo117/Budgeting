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

        public decimal Total()
        {
            if (Transactions == null || !Transactions.Any())
                return 0;
            
            return Transactions.Select(trans => trans.Amount)
                .Sum();
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