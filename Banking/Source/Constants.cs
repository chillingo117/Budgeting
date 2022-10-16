using System;
using System.IO;

namespace Banking.Source
{
    public class Constants
    {
        public static readonly string BucketDataFilename =
            String.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "buckets.csv");

        public static readonly string OtherPartyAccountColumnName = "Other Party Account";

        public static readonly string SummaryFileName =
            String.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "summary.csv");
    }
}