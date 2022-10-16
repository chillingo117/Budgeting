using System.Globalization;
using System.IO;
using System.Linq;
using Banking.Source;
using CsvHelper;
using Eto.Forms;

namespace Banking.UI
{
    public class SaveSummaryCommand: Command
    {
        public SaveSummaryCommand(Sorter sorter)
        {
            ToolBarText = "Save Summary";
            Executed += (sender, e) =>
            {
                var outputDirectory = Constants.SummaryFileName;

                var toWrite = sorter.Buckets.SelectMany(b => b.CategoriseBuckets());
                using(var writer = new StreamWriter(outputDirectory))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(toWrite);
                }
            };
        }
    }
}