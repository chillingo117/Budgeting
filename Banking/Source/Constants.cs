using System;
using System.IO;

namespace Banking.Source
{
    public class Constants
    {
        public static string BucketDataFilename =
            String.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "buckets.csv");
    }
}