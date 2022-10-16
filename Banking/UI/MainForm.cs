using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Banking.Source;
using CsvHelper;
using Eto.Drawing;
using Eto.Forms;
using Plotly.NET;

namespace Banking.UI
{
    public class MainForm : Form
    { 
        public MainForm()
        {
            Sorter = new Sorter();
            SorterUi = new SorterUi(Sorter, RefreshUi);
            Title = "It's Budgeting Time";
            MinimumSize = new Size(1200, 600);

            RefreshUi();

            ToolBar = new ToolBar();
            AttachOpenFileDialog();
            AttachAddBucketDialog();
            AttachSaveSummaryDialog();
            
            Closed += OnClosed;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            var outputDirectory = Constants.BucketDataFilename;

            var toWrite = Sorter.Buckets.Select(b => new BucketName {Name = b.Name}).ToList();;
            
            using (var writer = new StreamWriter(outputDirectory))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(toWrite);
            }
        }
        
        private Sorter Sorter { get; }
        private SorterUi SorterUi { get; }
        private string OpenedFilename;
        
        private readonly Func<TableRow> _makeHeaderRow = 
            () => new TableRow(
            new TableCell(new Label {Text = "Date"}, true),
            new TableCell(new Label {Text = "Amount"}, true),
            new TableCell(new Label {Text = "Payee"}, true),
            new TableCell(new Label {Text = "Particulars"}, true),
            new TableCell(new Label {Text = "Code"}, true),
            new TableCell(new Label {Text = "Reference"}, true),
            new TableCell(new Label {Text = "Other Party Account"}, true)
            );
        
        private void AttachOpenFileDialog()
        {
            var chooseFile = new Command {MenuText = "Open File", ToolBarText = "Open File"};
            chooseFile.Executed += (sender, e) => {
                var dialog = new OpenFileDialog
                {
                    MultiSelect = false,
                    Filters = { new FileFilter("csvs", ".csv") },   
                };
                dialog.ShowDialog(this);
                
                OpenedFilename = dialog.Filenames.FirstOrDefault();
                if (OpenedFilename != null)
                    Sorter.LoadData(OpenedFilename);
                RefreshUi();
            };
            
            ToolBar.Items.Add(chooseFile);
        }

        private void AttachAddBucketDialog()
        {
            var addBucket = new Command {ToolBarText = "Add bucket"};
            addBucket.Executed += (sender, e) => {     
                var dialog = new AddBucketDialog();
                dialog.ShowModal(this);
                Sorter.AddBucket(dialog.Result);
                RefreshUi();
            };
            ToolBar.Items.Add(addBucket);
        }

        private void AttachSaveSummaryDialog()
        {
            var saveSummary = new SaveSummaryCommand(Sorter);
            ToolBar.Items.Add(saveSummary);
        }
        
        private TableRow GetCurrentTransactionTableRow()
        {
            var currentTransaction = Sorter.CurrentTransaction;
            if (currentTransaction != null)
            {
                var amountCell = new TableCell(new Label {Text = currentTransaction.Amount.ToString(CultureInfo.InvariantCulture)}, true);
                amountCell.Control.BackgroundColor = currentTransaction.Amount < 0 ? Colors.OrangeRed : Colors.LimeGreen;
                return new TableRow(
                    new TableCell(new Label {Text = currentTransaction.Date.ToString(CultureInfo.InvariantCulture)}, true),
                    amountCell,
                    new TableCell(new Label {Text = currentTransaction.Payee}, true),
                    new TableCell(new Label {Text = currentTransaction.Particulars}, true),
                    new TableCell(new Label {Text = currentTransaction.Code}, true),
                    new TableCell(new Label {Text = currentTransaction.Reference}, true),
                    new TableCell(new Label {Text = currentTransaction.OtherPartyAccount}, true)
                );
            }
            return new TableRow();
        }

        private TableRow GetSorterUiRow()
        {
            return  new TableRow( new TableCell(SorterUi.Layout(), true));
        }

        private void RefreshUi()
        {
            var table = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = {
                    _makeHeaderRow(),
                    GetCurrentTransactionTableRow()
                }
            };
            var buckets = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows =
                {
                    GetSorterUiRow()
                }
            };
            var overallStack = new StackLayout();
            overallStack.Items.Add(table);
            overallStack.Items.Add(buckets);
            Content = overallStack;
        }
    }
}