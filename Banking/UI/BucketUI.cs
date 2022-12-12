using System;
using Banking.Source;
using Eto.Forms;

namespace Banking.UI
{
    public class BucketUi
    {
        public BucketUi(Sorter.Bucket bucket, Action<string> addTransactionToBucketAction, Action<string>addAllTransactionsToBucketAction , Action<string> removeBucketAction)
        {
            Bucket = bucket;

            var addToBucketButton = new Button{Text = Bucket.Name};
            addToBucketButton.Click += delegate
            {
                if (Keyboard.IsKeyLocked(Keys.CapsLock))
                    addAllTransactionsToBucketAction(Bucket.Name);
                else
                    addTransactionToBucketAction(Bucket.Name);
            };
            var removeBucketButton = new Button {Text = "remove"};
            removeBucketButton.Click += delegate
            {
                removeBucketAction(Bucket.Name);
            };
            
            Layout = new TableLayout
            {
                Rows =
                {
                    new TableRow( new TableCell(addToBucketButton)),
                    new TableRow( new TableCell(removeBucketButton))
                }
            };
        }

        private Sorter.Bucket Bucket { get; }
        public TableLayout Layout { get; }
    }
}