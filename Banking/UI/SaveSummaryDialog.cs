using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Banking.Source;
using CsvHelper;
using Eto.Forms;
using Source;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

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
                    csv.Context.RegisterClassMap<CategorisedTransactionCsvMap>();
                    csv.WriteRecords(toWrite);
                }
                RunSync();
            };
        }
        
        private void RunSync()
        {
            var Command = "C:\\phocas\\PhocasSyncCli.exe -run C:\\stash\\Banking\\Banking\\Source\\Client\\Banking.sync " +
                          "-service http://localhost:8080/ " +
                          "-username phocas " +
                          "-password phocas " +
                          $"-p:BasePath {Constants.BasePath}";
            var ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;

            Process.Start(ProcessInfo);
        }
    }
}