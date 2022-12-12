using System;
using System.Collections.Generic;
using System.Globalization;
using Banking.Source;
using Eto.Drawing;
using Eto.Forms;

namespace Banking.UI
{
    public class SorterUi
    {
        public SorterUi(Sorter sorter, Action refreshUi)
        {
            Sorter = sorter;
            ToolBar = new ToolBar();
            SearchInput = new TextBox{Text = Sorter.SearchFilter};
            AttachAddBucketDialog();
            _refreshUi = refreshUi;
        }

        private Sorter Sorter { get; }
        public ToolBar ToolBar { get; }
        private TextBox SearchInput { get; }
        
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
            overallStack.Items.Add( FilterBySearchControlRow());
            overallStack.Items.Add(GetPreviewTransactionsLayout());
            return overallStack;
        }
        
        private TableLayout FilterBySearchControlRow()
        {
            SearchInput.KeyUp += (sender, args) =>
            {
                if (args.Key == Keys.Enter)
                {
                    Sorter.SearchFilter = SearchInput.Text;
                    _refreshUi();
                }
            };

            return new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = {
                    new TableRow(
                        new TableCell(new Label{Text = "Search"}),
                        new TableCell(SearchInput, true)
                    )
                }
            };
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
            
            var transactions = String.IsNullOrWhiteSpace(Sorter.SearchFilter)
                ? Sorter.Transactions
                : Sorter.GetFilteredTransactions();
            
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
        
        private StackLayout BucketsLayout()
        {
            var layout = new StackLayout{Width = 1200, Spacing = 10};

            var count = 0;
            var subLayoutRow = new StackLayout{Width = 1200, Orientation = Orientation.Horizontal, Spacing = 5};

            foreach (var bucket in Sorter.Buckets)
            {
                subLayoutRow.Items.Add(new BucketUi(bucket, AddCurrentTransactionToBucket, AddAllTransactionsToBucket, RemoveBucket).Layout);
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

        private void AddCurrentTransactionToBucket(string name)
        {
            Sorter.AssignTransactionToBucket(name);
            _refreshUi();
        }

        private void AddAllTransactionsToBucket(string name)
        {
            Sorter.AssignAllTransactionsToBucket(name);
            _refreshUi();
        }

        private void RemoveBucket(string name)
        {
            Sorter.RemoveBucket(name);
            _refreshUi();
        }
    }
}