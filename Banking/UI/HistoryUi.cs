using System;
using System.Collections.Generic;
using System.Globalization;
using Banking.Source;
using Eto.Drawing;
using Eto.Forms;
using Source;

namespace Banking.UI
{
    public class HistoryUi
    {
        public HistoryUi(Sorter sorter, Action refreshUi)
        {
            Sorter = sorter;
            ToolBar = new ToolBar();
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
            new TableCell(new Label {Text = "Other Party Account"}, true),
            new TableCell(new Label {Text = "Bucket"}, true)
        );
        
        public StackLayout Layout()
        {
            var table = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = {
                    HeaderRow
                }
            };
            GetSortedTransactions().ForEach(row => table.Rows.Add(row));

            var layout = new StackLayout{Width = 1200, Spacing = 10};
            layout.Items.Add(table);
            return layout;
        }

        private List<TableRow> GetSortedTransactions()
        {
            var rows = new List<TableRow>();
            var sortedTransactions = Sorter.SortedTransactions;
            sortedTransactions.ForEach(st => rows.Add(GetSortedTransactionRow(st)));
            return rows;
        }

        private TableRow GetSortedTransactionRow(SortedTransaction transaction)
        {
            var amountCell = new TableCell(new Label {Text = transaction.Amount.ToString(CultureInfo.InvariantCulture)}, true);
            var undoButton = new Button {Text = "undo"};
            undoButton.Click += delegate
            {
                Sorter.UndoTransactionSort(transaction.Guid);
                _refreshUi();
            };
            return new TableRow(
                new TableCell(new Label {Text = transaction.Date.ToString(CultureInfo.InvariantCulture)}, true),
                amountCell,
                new TableCell(new Label {Text = transaction.Payee}, true),
                new TableCell(new Label {Text = transaction.Particulars}, true),
                new TableCell(new Label {Text = transaction.Code}, true),
                new TableCell(new Label {Text = transaction.Reference}, true),
                new TableCell(new Label {Text = transaction.OtherPartyAccount}, true),
                new TableCell(new Label {Text = transaction.Bucket}, true),
                new TableCell(undoButton, true)
            );
        }
    }
}