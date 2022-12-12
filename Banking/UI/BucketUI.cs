using System;
using Eto.Forms;

namespace Banking.UI
{
    public class BucketUi
    {
        public BucketUi(string bucket, Action<string> addTransactionToBucketAction, Action<string>addAllTransactionsToBucketAction , Action<string> removeBucketAction)
        {
            Bucket = bucket;

            var addToBucketButton = new Button{Text = Bucket};
            addToBucketButton.Click += delegate
            {
                if (Keyboard.IsKeyLocked(Keys.CapsLock))
                    addAllTransactionsToBucketAction(Bucket);
                else
                    addTransactionToBucketAction(Bucket);
            };
            var removeBucketButton = new Button {Text = "remove"};
            removeBucketButton.Click += delegate
            {
                removeBucketAction(Bucket);
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

        private string Bucket { get; }
        public TableLayout Layout { get; }
    }
}