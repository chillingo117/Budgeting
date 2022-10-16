using System;
using System.IO;

namespace Banking.Source
{
    public class Constants
    {
        public static readonly string BasePath = Directory.GetCurrentDirectory();

        public static readonly string BucketDataFilename = Path.Combine(BasePath, "buckets.csv");
        
        public static readonly string OtherPartyAccountColumnName = "Other Party Account";

        public static readonly string SummaryFileName = Path.Combine(BasePath, "summary.csv");
    }
}