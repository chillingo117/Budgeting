using System;
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
            AttachAddBucketDialog();
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
            var table = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = {
                    HeaderRow,
                    GetCurrentTransactionTableRow()
                }
            };
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
            overallStack.Items.Add(table);
            overallStack.Items.Add(buckets);
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

        private void RemoveBucket(string name)
        {
            Sorter.RemoveBucket(name);
            _refreshUi();
        }
    }
}