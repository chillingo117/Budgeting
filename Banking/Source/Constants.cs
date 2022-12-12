using System.Collections.Generic;
using System.IO;

namespace Banking.Source
{
    public class Constants
    {
        public static readonly string BasePath = Directory.GetCurrentDirectory();

        public static readonly string BucketDataFilename = Path.Combine(BasePath, "buckets.csv");
        
        public static readonly string OtherPartyAccountColumnName = "Other Party Account";

        public static readonly string SummaryFileName = Path.Combine(BasePath, "summary.csv");

        public static readonly string TestArg = "-test";
        
        public static readonly string ConfigName = "Banking.sync";
        
        public static readonly string TestConfigName = "TestBanking.sync";

        public static readonly List<string> Buckets = new List<string>
        {
            "Other",
            "Unknown",
            "Tech",
            "Alcohol",
            "Personal Maintenance",
            "Subscriptions",
            "Entertainment",
            "Clothing",
            "Car",
            "Hobbies",
            "Flat",
            "Food",
            "Travel",
            "Internal"
        };
    }
}