using System.Diagnostics;
using System.Globalization;
using System.IO;
using Banking.Source;
using CsvHelper;
using Eto.Forms;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace Banking.UI
{
    public class SaveSummaryCommand: Command
    { 
        public SaveSummaryCommand(Sorter sorter, bool isTest = false)
        {
            MenuText = "Save Summary";
            Executed += (sender, e) =>
            {
                var outputDirectory = Constants.SummaryFileName;

                var toWrite = sorter.SortedTransactions;
                using(var writer = new StreamWriter(outputDirectory))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<SortedTransactionCsvMap>();
                    csv.WriteRecords(toWrite);
                }
                RunSync(isTest);
            };
        }
        
        private static void RunSync(bool isTest)
        {
            var configName = isTest ? Constants.TestConfigName : Constants.ConfigName;
            var command = $"C:\\phocas\\PhocasSyncCli.exe -run C:\\stash\\Banking\\Banking\\Source\\Client\\{configName} " +
                          "-service http://localhost:8080/ " +
                          "-username phocas " +
                          "-password phocas " +
                          $"-p:BasePath {Constants.BasePath}";
            var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = true;

            Process.Start(processInfo);
        }
    }
}