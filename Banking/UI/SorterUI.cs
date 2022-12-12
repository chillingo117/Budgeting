using System;
using System.Collections.Generic;
using System.Globalization;
using Banking.Source;
using Eto.Drawing;
using Eto.Forms;
using Source;

namespace Banking.UI
{
    public class SorterUi
    {
        public SorterUi(Sorter sorter, Action refreshUi)
        {
            Sorter = sorter;
            ToolBar = new ToolBar();
            AttachAddBucketDialog();
            AttachSortByRegexDialog();
            _refreshUi = refreshUi;
        }

        private Sorter Sorter { get; }
        public ToolBar ToolBar { get; }
        
        private readonly Action _refreshUi;
        private TableRow HeaderRow => new TableRow(
            new TableCell(new Label {Text = "Date"}, true),
            new TableCell(new Label {Text = "Amount"}, true),
            new TableCell(new Label {Text = "Payee"}, true),
            new TableCell(new Label {Text = "Particulars"}, true),
            new TableCell(new Label {Text = "Code"}, true),
            new TableCell(new Label {Text = "Reference"}, true),
            new TableCell(new Label {Text = "Other Party Account"}, true)
        );
        
        public StackLayout Layout()
        {
            var buckets = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow( new TableCell(BucketsLayout(), true))
                }
            };
            var overallStack = new StackLayout();
            overallStack.Items.Add(buckets);
            overallStack.Items.Add(GetPreviewTransactionsLayout());
            return overallStack;
        }

        private StackLayout BucketsLayout()
        {
            var layout = new StackLayout{Width = 1200, Spacing = 10};

            var count = 0;
            var subLayoutRow = new StackLayout{Width = 1200, Orientation = Orientation.Horizontal, Spacing = 5};

            foreach (var bucket in Sorter.Buckets)
            {
                subLayoutRow.Items.Add(new BucketUi(bucket, AddCurrentTransactionToBucket, RemoveBucket).Layout);
                if (count++ > 10)
                {
                    count = 0;
                    layout.Items.Add(subLayoutRow);
                    subLayoutRow = new StackLayout{Width = 1200, Orientation = Orientation.Horizontal, Spacing = 5};
                }
            }
            layout.Items.Add(subLayoutRow);
            return layout;
        }
        
        private void AttachAddBucketDialog()
        {
            var addBucket = new Command {ToolBarText = "Add Bucket"};
            addBucket.Executed += (sender, e) => {     
                var dialog = new AddBucketDialog();
                dialog.ShowModal();
                Sorter.AddBucket(dialog.Result);
                _refreshUi();
            };
            ToolBar.Items.Add(addBucket);
        }
        
        private void AttachSortByRegexDialog()
        {
            var sortByRegex = new Command {ToolBarText = "Sort by Regex"};
            sortByRegex.Executed += (sender, e) => {     
                var dialog = new SortByRegexDialog();
                dialog.ShowModal();
                Sorter.RegexFilter = dialog.Result;
                _refreshUi();
            };
            ToolBar.Items.Add(sortByRegex);
        }
        
        private StackLayout GetPreviewTransactionsLayout()
        {
            var table = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = {
                    HeaderRow
                }
            };
            
            var transactions = String.IsNullOrWhiteSpace(Sorter.RegexFilter)
                ? Sorter.Transactions
                : Sorter.GetRegexTransactions();
            
            var rows = new List<TableRow>();
            transactions.ForEach(st => rows.Add(GetPreviewTransactionsRow(st)));
            rows.ForEach(row => table.Rows.Add(row));

            var overallStack = new StackLayout();
            overallStack.Items.Add(table);
            return overallStack;        
        }

        private TableRow GetPreviewTransactionsRow(Transaction transaction)
        {
            var amountCell = new TableCell(new Label {Text = transaction.Amount.ToString(CultureInfo.InvariantCulture)}, true);
            amountCell.Control.BackgroundColor = transaction.Amount < 0 ? Colors.OrangeRed : Colors.LimeGreen;
            return new TableRow(
                new TableCell(new Label {Text = transaction.Date.ToString(CultureInfo.InvariantCulture)}, true),
                amountCell,
                new TableCell(new Label {Text = transaction.Payee}, true),
                new TableCell(new Label {Text = transaction.Particulars}, true),
                new TableCell(new Label {Text = transaction.Code}, true),
                new TableCell(new Label {Text = transaction.Reference}, true),
                new TableCell(new Label {Text = transaction.OtherPartyAccount}, true)
            );
        }

        private void AddCurrentTransactionToBucket(string name)
        {
            Sorter.AssignTransactionToBucket(name);
            _refreshUi();
        }

        private void RemoveBucket(string name)
        {
            Sorter.RemoveBucket(name);
            _refreshUi();
        }
    }
}